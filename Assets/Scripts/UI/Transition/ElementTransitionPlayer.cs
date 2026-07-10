using System;
using TMPro;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ElementTransitionPlayer : BaseTransitionPlayer
    {
        [HideInInspector] public TextMeshProUGUI tmp;
        
        [HideInInspector] public Vector3 halfScale;
        [HideInInspector] public float halfAlpha;

        public override void Init()
        {
            base.Init();

            tmp = GetComponent<TextMeshProUGUI>();
            
            halfScale = new Vector3(
                hiddenScale.x + (showScale.x - hiddenScale.x) / 2f,
                hiddenScale.y + (showScale.y - hiddenScale.y) / 2f,
                showScale.z
            );
            halfAlpha = hiddenAlpha + (showAlpha - hiddenAlpha) / 2f;
        }

        public void PlayCustom(Vector2 targetPos, Vector3 targetScale, float targetAlpha, Action onComplete = null, float timeDivider = 1)
        {
            Vector2 startPos = rectTransform.anchoredPosition;
            Vector3 startScale = rectTransform.localScale;
            float startAlpha = canvasGroup.alpha;

            animator.PlayAnim(onComplete, startPos, targetPos, startScale, targetScale, startAlpha, targetAlpha, duration / timeDivider, useSmoothing, hasSlide, hasScale, hasFade, true);
        }

        public void PlayToPosAndShow(Vector2 targetPos, bool isCenter, Action onComplete = null, float timeDivider = 1)
        {
            Vector3 targetScale = isCenter ? showScale : halfScale;
            float targetAlpha = isCenter ? showAlpha : halfAlpha;
            PlayCustom(targetPos, targetScale, targetAlpha, onComplete, timeDivider);
        }

        public void ApplyHalfInstant()
        {
            animator.PlayInstant(showPos, halfScale, halfAlpha, hasSlide, hasScale, hasFade, true);
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