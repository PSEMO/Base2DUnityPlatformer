using System;
using System.Collections;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UIAnimator : MonoBehaviour
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private bool isInit = false;

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            if(isInit)
                return;
            else
                isInit = true;

            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void PlayInstant(Vector2 endPos, Vector3 endScale, float endAlpha,
            bool animateSlide, bool animateScale, bool animateFade,
            bool show)
        {
            StopAllCoroutines();

            if (animateSlide) rectTransform.anchoredPosition = endPos;
            if (animateScale) rectTransform.localScale = endScale;
            if (animateFade) canvasGroup.alpha = endAlpha;
        }

        public void PlayAnim(Action onComplete,
            Vector2 startPos, Vector2 endPos,
            Vector3 startScale, Vector3 endScale,
            float startAlpha, float endAlpha,
            float duration,
            bool useSmoothing, bool animateSlide, bool animateScale, bool animateFade,
            bool show)
        {
            StartCoroutine(TransitionRoutine(onComplete, startPos, endPos, startScale, endScale, startAlpha, endAlpha, duration, useSmoothing, animateSlide, animateScale, animateFade));
        }

        private IEnumerator TransitionRoutine(Action onComplete, Vector2 startPos, Vector2 endPos, Vector3 startScale, Vector3 endScale, float startAlpha, float endAlpha, float duration, bool useSmoothing, bool animateSlide, bool animateScale, bool animateFade)
        {
            float elapsed = 0f;

            if (duration > 0f)
            {
                while (elapsed < duration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float t = Mathf.Clamp01(elapsed / duration);

                    if (useSmoothing)
                    {
                        t = Mathf.SmoothStep(0, 1, t);
                    }

                    if (animateSlide) rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                    if (animateScale) rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                    if (animateFade) canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

                    yield return null;
                }
            }

            if (animateSlide) rectTransform.anchoredPosition = endPos;
            if (animateScale) rectTransform.localScale = endScale;
            if (animateFade) canvasGroup.alpha = endAlpha;

            onComplete?.Invoke();
        }
    }
}