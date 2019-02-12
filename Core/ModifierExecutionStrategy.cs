using System;
using System.Collections.Generic;
using VenusParticleEngine.Core.Modifiers;

namespace VenusParticleEngine.Core
{
    using TPL = System.Threading.Tasks;

    [Serializable]
    public class ModifierExecutionStrategy
    {
        public string Name = "";
        public enum EnumModifierExecutionStrategy { Serial, Parallel };

        public ModifierExecutionStrategy()
        {
            Name = ToString();
        }

        public virtual void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
        }

        public override string ToString()
        {
            int startNameProfile = GetType().ToString().LastIndexOf(".") + 1;
            string profileName = GetType().ToString().Substring(startNameProfile);
            int endNameProfile = profileName.IndexOf("Mo");
            profileName = profileName.Substring(0, endNameProfile);

            if (profileName.Equals(""))
            {
                profileName = "NOT_SET";
            }

            return profileName;
        }

        public static ModifierExecutionStrategy Serial() { return new SerialModifierExecutionStrategy(); }
        public static ModifierExecutionStrategy Parallel() { return new ParallelModifierExecutionStrategy(); }
    }


    [Serializable]
    internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
    {
        public override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            TPL.Parallel.ForEach(modifiers, modifier => modifier.Value.Update(elapsedSeconds, iterator.Reset()));
        }
    }

    [Serializable]
    internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
    {
        public override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            foreach (var modifier in modifiers)
                modifier.Value.Update(elapsedSeconds, iterator.Reset());
        }
    }
}