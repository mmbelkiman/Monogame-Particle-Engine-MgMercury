﻿namespace VenusParticleEngine.Core.Modifiers
{
    /// <summary>
    /// Defines a modifier which interpolates the colour of a particle over the course of its lifetime.
    /// </summary>
    public sealed class ColourInterpolator2 : IModifier
    {
        public string Name = "ColourInterpolator2";

        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        public Colour InitialColour { get; set; }

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public Colour FinalColour { get; set; }

        public ColourInterpolator2() { }

        public ColourInterpolator2(Colour initialColour, Colour finalColour)
        {
            InitialColour = initialColour;
            FinalColour = finalColour;
        }

        public unsafe void Update(float elapsedseconds, ParticleBuffer.ParticleIterator iterator)
        {
            var delta = new Colour(FinalColour.H - InitialColour.H,
                                   FinalColour.S - InitialColour.S,
                                   FinalColour.L - InitialColour.L);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Colour = new Colour(
                    InitialColour.H + delta.H * particle->Age,
                    InitialColour.S + delta.S * particle->Age,
                    InitialColour.L + delta.L * particle->Age);
            }
        }
    }
}