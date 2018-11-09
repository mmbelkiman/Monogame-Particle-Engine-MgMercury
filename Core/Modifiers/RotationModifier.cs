namespace MonoGameMPE.Core.Modifiers
{
    public class RotationModifier : IModifier
    {
        public string Name = "RotationModifier";
        public float RotationRate { get; set; }

        public RotationModifier() { }

        public RotationModifier(float rotationRate)
        {
            RotationRate = rotationRate;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var rotationRateDelta = RotationRate * elapsedSeconds;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Rotation += rotationRateDelta;
            }
        }
    }
}