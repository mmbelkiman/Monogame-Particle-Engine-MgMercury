using MonoGameMPE.Core.Modifiers.Container;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MonoGameMPE.Core.Modifiers
{
    public interface IModifier
    {
        void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator);
    }

    public class IModifierJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        { return (objectType == typeof(IModifier)); }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);

            switch (item["Name"].Value<string>())
            {
                case "CircleContainerModifier":
                    return new CircleContainerModifier((float)item["radius"], (bool)item["inside"], (float)item["restitutionCoefficient"]);
                case "RectContainerModifier":
                    return new RectContainerModifier((int)item["width"], (int)item["height"], (float)item["restitutionCoefficient"]);
                case "RectLoopContainerModifier":
                    return new RectLoopContainerModifier((int)item["width"], (int)item["height"]);
                case "ColourInterpolator2":
                    return new ColourInterpolator2(
                        new Colour((float)item["InitialColour"]["H"], (float)item["InitialColour"]["S"], (float)item["InitialColour"]["L"]),
                        new Colour((float)item["FinalColour"]["H"], (float)item["FinalColour"]["S"], (float)item["FinalColour"]["L"]));
                case "DragModifier":
                    return new DragModifier((float)item["DragCoefficient"], (float)item["Density"]);
                case "HueInterpolator2":
                    return new HueInterpolator2((float)item["InitialHue"], (float)item["FinalHue"]);
                case "LinearGravityModifier":
                    return new LinearGravityModifier(
                        new Axis((float)item["Direction"]["X"], (float)item["Direction"]["Y"]),
                        (float)item["Strength"]);
                case "OpacityFastFadeModifier":
                    return new OpacityFastFadeModifier();
                case "OpacityInterpolator2":
                    return new OpacityInterpolator2((float)item["InitialOpacity"], (float)item["MediumOpacity"], (float)item["FinalOpacity"]);
                case "RotationModifier":
                    return new RotationModifier((float)item["RotationRate"]);
                case "ScaleInterpolator2":
                    return new ScaleInterpolator2(
                        new Vector((float)item["InitialScale"]["X"], (float)item["InitialScale"]["Y"]),
                        new Vector((float)item["FinalScale"]["X"], (float)item["FinalScale"]["Y"]));

                default:
                    throw new NotImplementedException();
            }

            //  return serializer.Deserialize(reader, typeof(ColourInterpolator2));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
            //serializer.Serialize(writer, value, typeof(ColourInterpolator2));
        }
    }
}