namespace VenusParticleEngine.Core.Profiles
{
    public class CircleProfile : Profile
    {
        public override void GetOffsetAndHeading(out Vector offset, out Axis heading)
        {
            var dist = FastRand.NextSingle(0f, Radius);

            FastRand.NextUnitVector(out heading);

            offset = Radiate == CircleRadiation.In
                ? new Vector(-heading.X * dist, -heading.Y * dist)
                : new Vector(heading.X * dist, heading.Y * dist);

            if (Radiate == CircleRadiation.None)
                FastRand.NextUnitVector(out heading);
        }
    }
}