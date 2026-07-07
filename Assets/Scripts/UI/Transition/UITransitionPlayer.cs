using System;
using System.Collections;
using UnityEngine;

namespace PSEMO.UI
{
    public class UITransitionPlayer : MonoBehaviour
    {
        private TransitionSO data;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private bool hasFade;
        private bool hasSlide;
        private bool hasScale;
        private bool useSmoothing;
        
        private float duration;
        
        private Vector2 hiddenScale;
        private float hiddenAlpha;
        
        private Vector2 showPos;
        private Vector3 showScale;
        private float showAlpha;

        public void Init(TransitionSO transitionData, RectTransform rect, CanvasGroup cg)
        {
            data = transitionData;
            rectTransform = rect;
            canvasGroup = cg;

            hasFade = (data.transitionType & TransitionType.Fade) != 0;
            hasSlide = (data.transitionType & TransitionType.Slide) != 0;
            hasScale = (data.transitionType & TransitionType.Scale) != 0;
            useSmoothing = data.useSmoothing;

            duration = data.duration;

            hiddenScale = new Vector2(data.hiddenScale, data.hiddenScale);
            hiddenAlpha = data.hiddenAlpha;

            showPos = rectTransform.anchoredPosition;
            showScale = rectTransform.localScale;
            showAlpha = canvasGroup.alpha;
        }

        public void Play(bool show, Action onComplete, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            StopAllCoroutines();

            if (show)
            {
                StartCoroutine(
                    TransitionRoutine(onComplete,
                    GetHiddenPos(overrideDirection), showPos,
                    hiddenScale, showScale,
                    hiddenAlpha, showAlpha));
            }
            else
            {
                StartCoroutine(
                    TransitionRoutine(onComplete,
                    showPos, GetHiddenPos(overrideDirection),
                    showScale, hiddenScale,
                    showAlpha, hiddenAlpha));
            }
        }

        public void ApplyInstant(bool show, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            StopAllCoroutines();

            if (show)
            {
                if (hasSlide) rectTransform.anchoredPosition = showPos;
                if (hasScale) rectTransform.localScale = showScale;
                if (hasFade) canvasGroup.alpha = showAlpha;
            }
            else
            {
                if (hasSlide) rectTransform.anchoredPosition = GetHiddenPos(overrideDirection);
                if (hasScale) rectTransform.localScale = hiddenScale;
                if (hasFade) canvasGroup.alpha = hiddenAlpha;
            }
        }

        private IEnumerator TransitionRoutine(Action onComplete,
            Vector2 startPos, Vector2 endPos,
            Vector3 startScale, Vector3 endScale,
            float startAlpha, float endAlpha)
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

                    if (hasSlide) rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                    if (hasScale) rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                    if (hasFade) canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

                    yield return null;
                }
            }

            if (hasSlide) rectTransform.anchoredPosition = endPos;
            if (hasScale) rectTransform.localScale = endScale;
            if (hasFade) canvasGroup.alpha = endAlpha;

            onComplete?.Invoke();
        }

        private Vector2 GetHiddenPos(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            Vector2 hiddenPos = showPos;

            SlideDirection resolvedDir = data.slideDirection;
            if (resolvedDir == SlideDirection.Auto)
                if (overrideDirection == SlideDirection.Auto)
                    resolvedDir = SlideDirection.Down;
                else
                    resolvedDir = overrideDirection;

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float dist = data.slideDistance;

            switch (resolvedDir)
            {
                case SlideDirection.Left: hiddenPos.x -= width * dist; break;
                case SlideDirection.Right: hiddenPos.x += width * dist; break;
                case SlideDirection.Up: hiddenPos.y += height * dist; break;
                case SlideDirection.Down: hiddenPos.y -= height * dist; break;
            };

            return hiddenPos;
        }

        public void PlayShow() => Play(true, null);

        public void PlayHide() => Play(false, null);
    }
}
