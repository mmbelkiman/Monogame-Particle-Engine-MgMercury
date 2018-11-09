namespace MonoGameMPE.Core.Modifiers
{
    public class HueInterpolator2 : IModifier
    {
        public string Name = "HueInterpolator2";

        public float InitialHue { get; set; }
        public float FinalHue { get; set; }

        public HueInterpolator2() { }

        public HueInterpolator2(float initialHue, float finalHue)
        {
            InitialHue = initialHue;
            FinalHue = finalHue;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var delta = FinalHue - InitialHue;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Colour = new Colour(
                    delta * particle->Age + InitialHue,
                    particle->Colour.S,
                    particle->Colour.L);
            }
        }
    }
}