using System.Collections.Generic;
using MonoGameMPE.Core.Modifiers;

namespace MonoGameMPE.Core
{
    using TPL = System.Threading.Tasks;

    public abstract class ModifierExecutionStrategy
    {
        internal abstract void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator);

        public static ModifierExecutionStrategy Serial = new SerialModifierExecutionStrategy();
        public static ModifierExecutionStrategy Parallel = new ParallelModifierExecutionStrategy();

        internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
        {
            public override string ToString()
            {
                return "Serial";
            }

            internal override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
            {
                foreach (var modifier in modifiers)
                    modifier.Value.Update(elapsedSeconds, iterator.Reset());
            }
        }

        internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
        {
            public override string ToString()
            {
                return "Parallel";
            }

            internal override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
            {
                TPL.Parallel.ForEach(modifiers, modifier => modifier.Value.Update(elapsedSeconds, iterator.Reset()));
            }
        }
    }
}