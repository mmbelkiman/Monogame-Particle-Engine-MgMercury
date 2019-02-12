﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using VenusParticleEngine.Core.Modifiers.Container;
using VenusParticleEngine.Core.Profiles;

namespace Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        ParticleEffect _particleEffectSquare;
        ParticleEffect _particleEffectImage;
        ParticleEffect _particleEffectJson;

        Vector _particleJsonPosition = new Vector(700, 0);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ParticleInitSquare();
            ParticleInitImage();
            ParticleInitJson();

            base.Initialize();
        }

        private void ParticleInitSquare()
        {
            _particleEffectSquare = new ParticleEffect();
            Texture2D blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });

            Dictionary<string, IModifier> Modifiers = new Dictionary<string, IModifier>
            {
                {
                    "effect1", new ColourInterpolator2
                    {
                        InitialColour = new Colour(0.33f, 0.5f, 0.5f),
                        FinalColour = new Colour(0f, 0.5f, 0.5f)
                    }
                },
                {
                     "effect2", new RotationModifier
                     {
                          RotationRate = 1f
                     }

                },
                {
                    "effect3", new RectContainerModifier
                    {
                        Height = 100,
                        Width = 100,
                    }
                }
            };

            _particleEffectSquare.Emitters.Add("test", new Emitter(2000, TimeSpan.FromSeconds(2), Profile.Point())
            {
                Texture = blankTexture,
                BlendMode = BlendMode.Alpha,
                Parameters = new ReleaseParameters
                {
                    Speed = new RangeF(20f, 50f),
                    Quantity = 3,
                },
                Modifiers = Modifiers
            });
        }

        private void ParticleInitImage()
        {
            _particleEffectImage = new ParticleEffect();
            Texture2D imageTexture = Content.Load<Texture2D>("heart");

            Dictionary<string, IModifier> Modifiers = new Dictionary<string, IModifier>
            {
                {
                     "effect1", new ScaleInterpolator2
                     {
                          InitialScale = new Vector(10,10),
                          FinalScale =  new Vector(20,20)
                     }

                },
                {
                     "effect2", new RotationModifier
                     {
                          RotationRate = 1f
                     }

                },
                {
                    "effect3", new LinearGravityModifier
                    {
                      Direction = new Axis(0,-10),
                      Strength = 100
                    }
                }
            };

            _particleEffectImage.Emitters.Add("test", new Emitter(200, TimeSpan.FromSeconds(2), Profile.Circle(50, Profile.CircleRadiation.Out))
            {
                Texture = imageTexture,
                BlendMode = BlendMode.Alpha,
                Parameters = new ReleaseParameters
                {
                    Speed = new RangeF(20f, 50f),
                    Quantity = 3,
                },
                Modifiers = Modifiers
            });
        }

        private void ParticleInitJson()
        {
            _particleEffectJson = ParticleEffect.ReadFromJsonFile(Content.RootDirectory + "\\testParticle.ptc", GraphicsDevice, Content);
            foreach (var item in _particleEffectJson.Emitters)
            {
                if (string.IsNullOrEmpty(item.Value.TexturePath))
                {
                    item.Value.Texture = new Texture2D(GraphicsDevice, 1, 1);
                    item.Value.Texture.SetData(new[] { Color.White });
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            //Square
            _particleEffectSquare.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectSquare.Trigger(new Vector(150, 350));

            //Image
            _particleEffectImage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectImage.Trigger(new Vector(400, 250));

            //Json
            _particleEffectJson.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleJsonPosition.Y += 10;
            _particleEffectJson.Trigger(_particleJsonPosition);

            if (_particleJsonPosition.Y > 1000)
            {
                _particleJsonPosition.Y = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            {
                _spriteBatch.Draw(_particleEffectSquare);
                _spriteBatch.Draw(_particleEffectImage);
                _spriteBatch.Draw(_particleEffectJson);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}