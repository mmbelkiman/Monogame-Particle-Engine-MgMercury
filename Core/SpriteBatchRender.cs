using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VenusParticleEngine.Core
{
    public static class SpriteBatchRender
    {
        public static void Draw(this SpriteBatch spriteBatch, ParticleEffect effect)
        {
            foreach (var emitter in effect.Emitters)
                DrawParticle(emitter.Value, spriteBatch);
        }

        private static unsafe void DrawParticle(Emitter emitter, SpriteBatch spriteBatch)
        {
            if (emitter.Texture == null) return;
            var texture = emitter.Texture;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            var blendState = emitter.BlendMode == BlendMode.Add
                ? BlendState.Additive
                : BlendState.AlphaBlend;

            var iterator = emitter.Buffer.Iterator;
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var color = particle->Colour.ToRgb();
                if (blendState == BlendState.AlphaBlend)
                    color *= particle->Opacity;
                else
                    color.A = (byte)(particle->Opacity * 255);

                spriteBatch.Draw(
                    texture,
                    new Vector2(particle->Position.X, particle->Position.Y),
                    null,
                    new Color(color, particle->Opacity),
                    particle->Rotation,
                    origin,
                    new Vector2(particle->Scale.X / texture.Width, particle->Scale.Y / texture.Height),
                    emitter.SpriteEffects,
                    emitter.LayerDepth);
            }
        }
    }
}