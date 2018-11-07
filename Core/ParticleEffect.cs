using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameMPE.Core
{
    [Serializable]
    public class ParticleEffect
    {
        public string Name { get; set; }
        public Dictionary<string, Emitter> Emitters { get; set; }
        private bool active = true;

        public ParticleEffect()
        {
            Emitters = new Dictionary<string, Emitter>();
        }

        public int ActiveParticles => Emitters.Sum(t => t.Value.ActiveParticles);

        public void FastForward(Vector position, float seconds, float triggerPeriod)
        {
            var time = 0f;
            while (time < seconds)
            {
                Update(triggerPeriod);
                Trigger(position);
                time += triggerPeriod;
            }
        }

        public void Update(float elapsedSeconds)
        {
            if (!active) return;
            foreach (var e in Emitters)
            {
                e.Value.Update(elapsedSeconds);
            }
        }

        public void Trigger(Vector position)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(position);
        }

        public void Trigger(LineSegment line)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(line);
        }

        public void Play()
        {
            active = true;
        }

        public void Pause()
        {
            active = false;
        }

        public void PlayPause()
        {
            active = !active;
        }

    }
}