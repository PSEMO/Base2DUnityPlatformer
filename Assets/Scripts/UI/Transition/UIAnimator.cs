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

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (isInit)
                return;
            else
                isInit = true; 

            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetPos(Vector2 posToSet) => rectTransform.anchoredPosition = posToSet;
        public void SetScale(Vector3 scaleToSet) => rectTransform.localScale = scaleToSet;
        public void SetAlpha(float alphaToSet) => canvasGroup.alpha = alphaToSet;

        public void PlayInstant(bool show,
            Vector2 endPos, Vector3 endScale, float endAlpha,
            bool playPos = true, bool playScale = true, bool playAlpha = true)
        {
            StopAllCoroutines();
            
            if(playPos) SetPos(endPos);
            if(playScale) SetScale(endScale);
            if(playAlpha) SetAlpha(endAlpha);

            SetInteraction(true);

            gameObject.SetActive(show);
        }

        public void PlayAnim(
            Action onComplete,
            Vector2 startPos, Vector2 endPos,
            Vector3 startScale, Vector3 endScale,
            float startAlpha, float endAlpha,
            float duration,
            bool show,
            bool playFade = true, bool playSlide = true, bool playScale = true, bool useSmoothing = true)
        {
            StopAllCoroutines();

            gameObject.SetActive(true);
            
            StartCoroutine(AnimRoutine(
                onComplete,
                startPos, endPos,
                startScale, endScale,
                startAlpha, endAlpha,
                duration,
                show,
                playFade, playSlide, playScale, useSmoothing));
        }

        private IEnumerator AnimRoutine(
            Action onComplete,
            Vector2 startPos, Vector2 endPos,
            Vector3 startScale, Vector3 endScale,
            float startAlpha, float endAlpha,
            float duration,
            bool show,
            bool playFade, bool playSlide, bool playScale, bool useSmoothing)
        {
            SetInteraction(false);

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

                    if (playFade) canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                    if (playSlide) rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                    if (playScale) rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);

                    yield return null;
                }
            }

            if (playFade) canvasGroup.alpha = endAlpha;
            if (playSlide) rectTransform.anchoredPosition = endPos;
            if (playScale) rectTransform.localScale = endScale;

            SetInteraction(true);

            gameObject.SetActive(show);
            onComplete?.Invoke();
        }

        private void SetInteraction(bool setTo)
        {
            canvasGroup.interactable = setTo;
            canvasGroup.blocksRaycasts = setTo;
        }
    }
}