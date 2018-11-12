namespace MonoGameMPE.Core.Modifiers
{
    public class OpacityInterpolator3 : IModifier
    {
        public string Name = "OpacityInterpolator3";

        public float InitialOpacity { get; set; }
        public float MediumOpacity { get; set; }
        public float FinalOpacity { get; set; }

        public OpacityInterpolator3() { }

        public OpacityInterpolator3(float initialOpacity, float mediumOpacity, float finalOpacity)
        {
            InitialOpacity = initialOpacity;
            MediumOpacity = mediumOpacity;
            FinalOpacity = finalOpacity;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var delta = MediumOpacity - InitialOpacity;
            var delta2 = FinalOpacity - MediumOpacity;
            bool mediumIsLowerInitial = (MediumOpacity - InitialOpacity) >= 0 ? false : true;
            bool finalIsLowerInitial = (FinalOpacity - MediumOpacity) >= 0 ? false : true;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                if (particle->Age == particle->StartAge)
                {
                    particle->Opacity = InitialOpacity;
                }

                if (particle->Age < particle->StartAge * ((particle->Term * 100) / 2))
                {
                    particle->Opacity = delta * ((particle->Age) * 1f) + InitialOpacity;
                }

                if (particle->Age >= particle->StartAge * ((particle->Term * 100) / 2))
                {
                    particle->Opacity = delta2 * ((particle->Age) * 1f) + MediumOpacity;
                }

                if (particle->Opacity < 0)
                    particle->Opacity = 0;
            }
        }
    }
}