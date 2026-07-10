using System;
using UnityEngine;

namespace PSEMO.UI
{
    public class ElementTransitionPlayer : BaseTransitionPlayer
    {
        public void PlayInstantTo(Vector2 endPos)
        {
            animator.PlayInstant(true, endPos, Vector3.one, 1f, hasSlide, hasScale, hasFade);
        }

        public void PlayInstantHiddenAt(Vector2 pos)
        {
            animator.PlayInstant(false, pos, hiddenScale, hiddenAlpha, hasSlide, hasScale, hasFade);
        }

        public void PlayTo(Vector2 endPos, Action onComplete = null, float timeDivider = 1)
        {
            animator.PlayAnim(onComplete,
                rectTransform.anchoredPosition, endPos,
                Vector3.one, Vector3.one,
                1f, 1f,
                duration / timeDivider,
                true,
                hasFade, hasSlide, hasScale, useSmoothing);
        }

        public void PlayHideTo(Vector2 endPos, Action onComplete = null, float timeDivider = 1)
        {
            animator.PlayAnim(onComplete,
                rectTransform.anchoredPosition, endPos,
                Vector3.one, hiddenScale,
                1f, hiddenAlpha,
                duration / timeDivider,
                false,
                hasFade, hasSlide, hasScale, useSmoothing);
        }

        public void PlayShowFrom(Vector2 startPos, Vector2 endPos, Action onComplete = null, float timeDivider = 1)
        {
            animator.PlayAnim(onComplete,
                startPos, endPos,
                hiddenScale, Vector3.one,
                hiddenAlpha, 1f,
                duration / timeDivider,
                true,
                hasFade, hasSlide, hasScale, useSmoothing);
        }
    }
}
