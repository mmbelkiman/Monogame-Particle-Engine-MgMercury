using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VenusParticleEngine.Core.Modifiers;

namespace VenusParticleEngine.Core
{
    [Serializable]
    public class ParticleEffect
    {
        public string Name { get; set; }
        public Dictionary<string, Emitter> Emitters { get; set; }
        private bool active = true;

        public ParticleEffect()
        {
            Emitters = new Dictionary<string, Emitter>();
        }

        public static ParticleEffect ReadFromJson(string jsonPath, string jsonString, GraphicsDevice graphicsDevice, ContentManager content)
        {
            try
            {
                var settings = new JsonSerializerSettings();

                settings.Converters.Add(new IModifierJsonConverter());
                ParticleEffect pf = JsonConvert.DeserializeObject<ParticleEffect>(jsonString, settings);
                pf.UpdateEmmitersTexture(jsonPath, graphicsDevice, content);
                return pf;
            }
            catch (Exception e)
            {
                Console.WriteLine("Venus Particle Engine : ParticleEffect => " + e.Message);
                return null;
            }
        }

        public static ParticleEffect ReadFromJsonFile(string filePath, GraphicsDevice graphicsDevice, ContentManager content)
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                var settings = new JsonSerializerSettings();
                var jsonPath = filePath.Substring(0, filePath.LastIndexOf("\\")) + "\\";

                settings.Converters.Add(new IModifierJsonConverter());
                ParticleEffect pf = JsonConvert.DeserializeObject<ParticleEffect>(fileContents, settings);
                pf.UpdateEmmitersTexture(jsonPath, graphicsDevice, content);
                return pf;
            }
            catch (Exception e)
            {
                Console.WriteLine("Venus Particle Engine : ParticleEffect => " + e.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void UpdateEmmitersTexture(String jsonPath, GraphicsDevice graphicsDevice, ContentManager content)
        {
            foreach (var item in Emitters)
            {
                item.Value.Parameters.Quantity = 1;

                if (item.Value.TexturePath.Equals(""))
                {
                    item.Value.Texture = new Texture2D(graphicsDevice, 1, 1);
                    // item.Value.Texture.SetData<Color>(new[] { Color.White });
                }
                else
                {
                    if (jsonPath.Equals(""))
                    {
                        item.Value.Texture = LoadImage(item.Value.TexturePath, graphicsDevice, content);
                    }
                    else
                    {
                        item.Value.Texture = LoadImage(jsonPath + item.Value.TexturePath, graphicsDevice, content);
                    }
                }

                if (item.Value.ModifierExecutionStrategy.Name.Equals("Parallel"))
                {
                    item.Value.ModifierExecutionStrategy = ModifierExecutionStrategy.Parallel();
                }
                else
                {
                    item.Value.ModifierExecutionStrategy = ModifierExecutionStrategy.Serial();
                }
            }
        }

        private Texture2D LoadImage(String fileName, GraphicsDevice graphicsDevice, ContentManager content)
        {
            if (fileName.Contains(".xnb"))
            {
                return LoadImageXNB(fileName, content);
            }
            else
            {
                return LoadImageNormal(fileName, graphicsDevice);
            }
        }

        private Texture2D LoadImageXNB(string fileName, ContentManager content)
        {
            Texture2D texture = null;
            try
            {
                texture = content.Load<Texture2D>(fileName.ToLower().Replace(".xnb", "").Replace("content\\", ""));
            }
            catch
            {
                Console.WriteLine("Cannot open file " + fileName);
                throw new FileNotFoundException("Cannot open file", fileName);
            }

            return texture;
        }

        private Texture2D LoadImageNormal(string fileName, GraphicsDevice graphicsDevice)
        {
            Texture2D spriteAtlas = null;
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open);
                spriteAtlas = Texture2D.FromStream(graphicsDevice, fileStream);
                spriteAtlas.Name = fileName;
                fileStream.Dispose();
            }
            catch
            {
                Console.WriteLine("Cannot open file " + fileName);
                throw new FileNotFoundException("Cannot open file", fileName);
            }
            return spriteAtlas;
        }

        public int ActiveParticles => Emitters.Sum(t => t.Value.ActiveParticles);

        public void FastForward(Vector position, float seconds, float triggerPeriod)
        {
            var time = 0f;
            while (time < seconds)
            {
                Update(triggerPeriod);
                Trigger(position);
                time += triggerPeriod;
            }
        }

        public void Update(float elapsedSeconds)
        {
            if (!active) return;
            foreach (var e in Emitters)
            {
                e.Value.Update(elapsedSeconds);
            }
        }

        public void Trigger(Vector position)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(position);
        }

        public void Trigger(LineSegment line)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(line);
        }

        public void Play()
        {
            active = true;
        }

        public void Pause()
        {
            active = false;
        }

        public void PlayPause()
        {
            active = !active;
        }
    }
}