using System;

namespace VenusParticleEngine.Core.Profiles
{
    [Serializable]
    public class Profile : ICloneable
    {
        public string Name = "";

        public Axis Axis = Axis.Up;
        public float Length = 100;
        public Axis Direction = Axis.Up;
        public float Spread = 100;
        public float Radius = 100;
        public CircleRadiation Radiate = CircleRadiation.In;
        public float Width = 100;
        public float Height = 100;

        public enum EnumEmitterProfiles { Profile, BoxProfile, BoxFillProfile, BoxUniformProfile, CircleProfile, LineProfile, PointProfile, RingProfile, SprayProfile };

        public Profile()
        {
            Name = ToString();
        }

        public virtual void GetOffsetAndHeading(out Vector offset, out Axis heading)
        {
            offset = Vector.Zero;
            FastRand.NextUnitVector(out heading);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public enum CircleRadiation
        {
            None,
            In,
            Out
        }

        public static Profile Point()
        {
            return new PointProfile();
        }

        public static Profile Line(Axis axis, float length)
        {
            return new LineProfile
            {
                Axis = axis,
                Length = length
            };
        }

        public static Profile Ring(float radius, CircleRadiation radiate)
        {
            return new RingProfile
            {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Box(float width, float height)
        {
            return new BoxProfile
            {
                Width = width,
                Height = height
            };
        }

        public static Profile BoxFill(float width, float height)
        {
            return new BoxFillProfile
            {
                Width = width,
                Height = height
            };
        }

        public static Profile BoxUniform(float width, float height)
        {
            return new BoxUniformProfile
            {
                Width = width,
                Height = height
            };
        }

        public static Profile Circle(float radius, CircleRadiation radiate)
        {
            return new CircleProfile
            {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Spray(Axis direction, float spread)
        {
            return new SprayProfile
            {
                Direction = direction,
                Spread = spread
            };
        }

        public override string ToString()
        {
            int startNameProfile = GetType().ToString().LastIndexOf(".") + 1;
            string profileName = GetType().ToString().Substring(startNameProfile);

            return profileName;
        }
    }
}
