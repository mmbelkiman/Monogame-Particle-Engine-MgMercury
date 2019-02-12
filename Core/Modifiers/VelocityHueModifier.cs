using System;
using Microsoft.Xna.Framework;

namespace VenusParticleEngine.Core.Modifiers
{
    public class VelocityHueModifier : IModifier
    {
        public string Name = "VelocityHueModifier";
        public float StationaryHue { get; set; }
        public float VelocityHue { get; set; }
        public float VelocityThreshold { get; set; }

        public VelocityHueModifier() { }

        public VelocityHueModifier(float stationaryHue, float velocityHue, float velocityThreshold)
        {
            StationaryHue = stationaryHue;
            VelocityHue = velocityHue;
            VelocityThreshold = velocityThreshold;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var velocity2 = particle->Velocity.LengthSq;

                float h;
                if (velocity2 >= velocityThreshold2)
                {
                    h = VelocityHue;
                }
                else
                {
                    var t = (float)Math.Sqrt(velocity2) / VelocityThreshold;
                    h = MathHelper.Lerp(StationaryHue, VelocityHue, t);
                }
                particle->Colour = new Colour(h, particle->Colour.S, particle->Colour.L);
            }
        }
    }
}