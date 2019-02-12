using System;

namespace VenusParticleEngine.Core.Profiles
{
    public class SprayProfile : Profile
    {
        public override void GetOffsetAndHeading(out Vector offset, out Axis heading)
        {
            var angle = Direction.Map((x, y) => (float)Math.Atan2(y, x));

            angle = FastRand.NextSingle(angle - Spread / 2f, angle + Spread / 2f);

            offset = Vector.Zero;
            heading = new Axis((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}