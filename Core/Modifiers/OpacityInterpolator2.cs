﻿using System;

namespace MonoGameMPE.Core.Modifiers
{
    public class OpacityInterpolator2 : IModifier
    {
        public float InitialOpacity { get; set; }
        public float MediumOpacity { get; set; }
        public float FinalOpacity { get; set; }

        private float LastInitialOpacity = 0;
        private float LastMediumOpacity = 0;
        private float LastFinalOpacity = 0;

        private bool doMedium = true;
        private bool atFinal = false;

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            if (LastInitialOpacity != InitialOpacity)
            {
                InitialOpacity = LastInitialOpacity;
                doMedium = true;
                atFinal = false;
            }
            if (LastMediumOpacity != InitialOpacity)
            {
                MediumOpacity = LastMediumOpacity;
                doMedium = true;
                atFinal = false;
            }
            if (LastFinalOpacity != InitialOpacity)
            {
                FinalOpacity = LastFinalOpacity;
                doMedium = true;
                atFinal = false;
            }

            var delta = MediumOpacity - InitialOpacity;
            var delta2 = FinalOpacity - MediumOpacity;
            bool mediumIsLowerInitial = (MediumOpacity - InitialOpacity) >= 0 ? false : true;
            bool mediumIsLowerFinal = (MediumOpacity - FinalOpacity) >= 0 ? false : true;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                if (!atFinal)
                {
                    if (doMedium)
                        particle->Opacity = delta * ((particle->Age) * 2) + InitialOpacity;
                    else
                    {
                        particle->Opacity = delta2 * ((particle->Age) * 1) + MediumOpacity;
                        if (particle->Opacity < 0) particle->Opacity = FinalOpacity;
                    }
                }

                if (doMedium)
                {
                    if (
                        (mediumIsLowerInitial && particle->Opacity <= MediumOpacity)
                        || (!mediumIsLowerInitial && particle->Opacity >= MediumOpacity)
                        )
                    {
                        doMedium = false;
                        particle->Opacity = MediumOpacity;
                    }
                }
                else
                {
                    if (
                    (mediumIsLowerFinal && particle->Opacity >= FinalOpacity)
                    || (!mediumIsLowerFinal && particle->Opacity <= FinalOpacity)
                    )
                    {
                        atFinal = true;
                    }
                }
            }
        }
    }
}