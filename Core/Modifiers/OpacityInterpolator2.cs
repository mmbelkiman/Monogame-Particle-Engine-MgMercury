namespace MonoGameMPE.Core.Modifiers
{
    public class OpacityInterpolator2 : IModifier
    {
        public string Name = "OpacityInterpolator2";
        public float InitialOpacity { get; set; }
        public float FinalOpacity { get; set; }

        public OpacityInterpolator2() { }

        public OpacityInterpolator2(float initialOpacity, float finalOpacity)
        {
            InitialOpacity = initialOpacity;
            FinalOpacity = finalOpacity;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var delta = FinalOpacity - InitialOpacity;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Opacity = delta * particle->Age + InitialOpacity;
            }
        }
    }
}