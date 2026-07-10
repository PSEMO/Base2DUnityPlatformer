using System;
using TMPro;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ElementTransitionPlayer : BaseTransitionPlayer
    {
        [HideInInspector] public TextMeshProUGUI tmp;

        public override void Init()
        {
            base.Init();

            tmp = GetComponent<TextMeshProUGUI>();
        }

        public void PlayCustom(Vector2 targetPos, Vector3 targetScale, float targetAlpha, Action onComplete = null, float timeDivider = 1)
        {
            Vector2 startPos = rectTransform.anchoredPosition;
            Vector3 startScale = rectTransform.localScale;
            float startAlpha = canvasGroup.alpha;

            animator.PlayAnim(onComplete, startPos, targetPos, startScale, targetScale, startAlpha, targetAlpha, duration / timeDivider, useSmoothing, hasSlide, hasScale, hasFade, true);
        }

        public void PlayToPosAndShow(Vector2 targetPos, Action onComplete = null, float timeDivider = 1)
        {
            PlayCustom(targetPos, showScale, showAlpha, onComplete, timeDivider);
        }

        public void UpdateShowPos()
        {
            showPos = rectTransform.anchoredPosition;
        }

        public void UpdateShowPos(Vector2 newShowPosition)
        {
            showPos = newShowPosition;
        }
    }
}