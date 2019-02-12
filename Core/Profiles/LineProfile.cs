namespace VenusParticleEngine.Core.Profiles
{
    public class LineProfile : Profile
    {
        public override void GetOffsetAndHeading(out Vector offset, out Axis heading)
        {
            var vect = Axis * FastRand.NextSingle(Length * -0.5f, Length * 0.5f);
            offset = new Vector(vect.X, vect.Y);
            FastRand.NextUnitVector(out heading);
        }
    }
}