using System;

namespace MonoGameMPE.Core.Modifiers
{
    public class VelocityColourInfiniteModifier : IModifier
    {
        public string Name = "VelocityColourInfiniteModifier";

        public Colour Colour1 { get; set; }
        public Colour Colour2 { get; set; }
        public float VelocityChangeColor { get; set; }

        private int TimePass = 0;
        private bool useColour1 = true;

        public VelocityColourInfiniteModifier() { }

        public VelocityColourInfiniteModifier(Colour colour1, Colour colour2, float velocityChangeColor)
        {
            Colour1 = colour1;
            Colour2 = colour2;
            VelocityChangeColor = velocityChangeColor;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            if (TimePass < VelocityChangeColor) TimePass++;
            else { TimePass = 0; }

            if (TimePass >= VelocityChangeColor)
            {
                useColour1 = !useColour1;
                TimePass = 0;
            }

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Colour = useColour1 ? Colour1 : Colour2;
            }
        }
    }
}
