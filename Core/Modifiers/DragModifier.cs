namespace MonoGameMPE.Core.Modifiers
{
    public class DragModifier : IModifier
    {
        public string Name = "DragModifier";

        public float DragCoefficient;
        public float Density;

        public DragModifier()
        {

        }

        public DragModifier(float dragCoefficient, float density)
        {
            DragCoefficient = dragCoefficient;
            Density = density;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var drag = -DragCoefficient * Density * particle->Mass * elapsedSeconds;

                particle->Velocity = new Vector(
                    particle->Velocity.X + particle->Velocity.X * drag,
                    particle->Velocity.Y + particle->Velocity.Y * drag);
            }
        }
    }
}