using System;
using UnityEngine;

namespace PSEMO.UI
{
    public class PanelTransitionPlayer : BaseTransitionPlayer
    {
        public void PlayInstant(bool show, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            if(show)
            {
                animator.PlayInstant(show, showPos, showScale, showAlpha, hasSlide, hasScale, hasFade);
            }
            else
            {
                animator.PlayInstant(show, GetHiddenPos(showPos, overrideDirection), hiddenScale, hiddenAlpha, hasSlide, hasScale, hasFade);
            }
        }

        public void Play(bool show, Action onComplete = null, SlideDirection overrideDirection = SlideDirection.Auto, float timeDivider = 1)
        {
            StopAllCoroutines();

            if (show)
            {
                animator.PlayAnim(onComplete,
                    GetHiddenPos(showPos, overrideDirection), showPos,
                    hiddenScale, showScale,
                    hiddenAlpha, showAlpha,
                    duration / timeDivider,
                    show,
                    hasFade, hasSlide, hasScale, useSmoothing);
            }
            else
            {
                animator.PlayAnim(onComplete,
                    showPos, GetHiddenPos(showPos, overrideDirection),
                    showScale, hiddenScale,
                    showAlpha, hiddenAlpha,
                    duration / timeDivider,
                    show,
                    hasFade, hasSlide, hasScale, useSmoothing);
            }
        }
    }
}