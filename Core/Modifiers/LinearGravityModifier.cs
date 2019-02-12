namespace VenusParticleEngine.Core.Modifiers
{
    public class LinearGravityModifier : IModifier
    {
        public string Name = "LinearGravityModifier";
        public Axis Direction { get; set; }
        public float Strength { get; set; }

        public LinearGravityModifier() { }
        public LinearGravityModifier(Axis direction, float strength)
        {
            Direction = direction;
            Strength = strength;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var vector = Direction * (Strength * elapsedSeconds);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Velocity = new Vector(
                    particle->Velocity.X + vector.X * particle->Mass,
                    particle->Velocity.Y + vector.Y * particle->Mass);
            }
        }
    }
}