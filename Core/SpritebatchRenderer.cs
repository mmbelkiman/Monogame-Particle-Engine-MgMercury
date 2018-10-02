using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameMPE.Core
{
    public class SpriteBatchRenderer
    {
        /// <summary>
        /// Draw a particle effect. This draw function calls spritebatch.Begin() and .End()
        /// </summary>
        public void Draw(ParticleEffect effect, SpriteBatch spriteBatch, Matrix cameraView, SpriteEffects effects, float layerDepth)
        {
            foreach (var emitter in effect.Emitters)
                Draw(emitter.Value, spriteBatch, cameraView, effects, layerDepth);
        }

        public void Draw(ParticleEffect effect, SpriteBatch s)
        {
            foreach (var emitter in effect.Emitters)
                Draw(emitter.Value, s);
        }

        private unsafe void Draw(Emitter emitter, SpriteBatch spriteBatch)
        {
            var texture = emitter.Texture;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            var blendState = emitter.BlendMode == BlendMode.Add
                ? BlendState.Additive
                : BlendState.AlphaBlend;

            //TODO var sortMode = emitter.RenderingOrder == RenderingOrder.BackToFront ?

            spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            {
                var iterator = emitter.Buffer.Iterator;
                while (iterator.HasNext)
                {
                    var particle = iterator.Next();
                    var color = particle->Colour.ToRgb();
                    if (blendState == BlendState.AlphaBlend) color *= particle->Opacity;
                    else color.A = (byte)(particle->Opacity * 255);

                    SpriteEffects effects = SpriteEffects.None;
                    float layerDepth = 0;

                    spriteBatch.Draw(
                        texture,
                        new Vector2(particle->Position.X, particle->Position.Y),
                        null,
                        new Color(color, particle->Opacity),
                        particle->Rotation,
                        origin,
                        new Vector2(particle->Scale.X / texture.Width, particle->Scale.Y / texture.Height),
                        effects,
                        layerDepth);
                }
            }
            spriteBatch.End();
        }

        private unsafe void Draw(Emitter emitter, SpriteBatch spriteBatch, Matrix cameraView, SpriteEffects effects, float layerDepth)
        {
            var texture = emitter.Texture;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            var blendState = emitter.BlendMode == BlendMode.Add
                ? BlendState.Additive
                : BlendState.AlphaBlend;

            //TODO var sortMode = emitter.RenderingOrder == RenderingOrder.BackToFront ?

            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, SamplerState.PointClamp, null, null, null, cameraView);
            {
                var iterator = emitter.Buffer.Iterator;
                while (iterator.HasNext)
                {
                    var particle = iterator.Next();
                    var color = particle->Colour.ToRgb();
                    if (blendState == BlendState.AlphaBlend) color *= particle->Opacity;
                    else color.A = (byte)(particle->Opacity * 255);

                    spriteBatch.Draw(
                        texture,
                        new Vector2(particle->Position.X, particle->Position.Y),
                        null,
                        new Color(color, particle->Opacity),
                        particle->Rotation,
                        origin,
                        new Vector2(particle->Scale.X / texture.Width, particle->Scale.Y / texture.Height),
                        effects,
                        layerDepth);
                }
            }
            spriteBatch.End();
        }
    }
}