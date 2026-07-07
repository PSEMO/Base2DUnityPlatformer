using System.Collections;
using UnityEngine;
using System;

namespace PSEMO.UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public abstract class BasePanel : MonoBehaviour
    {
        [SerializeField] protected TransitionSO data;

        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;

        protected bool isOpen;
        [HideInInspector] public bool IsOpen
        {
            get => isOpen;
        }
        
        protected bool hasFade;
        protected bool hasSlide;
        protected bool hasScale;
        protected bool useSmoothing;
        
        protected float duration;
        
        protected Vector2 hiddenScale;
        protected float hiddenAlpha;
        
        protected readonly Vector2 showPos = Vector2.zero;
        protected readonly Vector2 showScale = Vector2.one;
        protected readonly float showAlpha = 1.0f;

        public virtual void Init()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            hasFade = (data.transitionType & TransitionType.Fade) != 0;
            hasSlide = (data.transitionType & TransitionType.Slide) != 0;
            hasScale = (data.transitionType & TransitionType.Scale) != 0;
            useSmoothing = data.useSmoothing;

            duration = data.duration;

            hiddenScale = new(data.hiddenScale, data.hiddenScale);
            hiddenAlpha = data.hiddenAlpha;

            if (hasFade) canvasGroup.alpha = hiddenAlpha;
            if (hasScale) rectTransform.localScale = new Vector3(hiddenScale.x, hiddenScale.y, 1f);
            if (hasSlide) rectTransform.anchoredPosition = GetHiddenPos();
            
            SetInteraction(false);
        }

        public abstract void Show(SlideDirection overrideDirection = SlideDirection.Auto);
        public abstract void Hide(SlideDirection overrideDirection = SlideDirection.Auto);

        protected void StartTransition(bool show, Action onComplete, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            SetInteraction(false);
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

        protected IEnumerator TransitionRoutine(Action onComplete,
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

        public void ShowInstant()
        {
            isOpen = true;

            StopAllCoroutines();

            gameObject.SetActive(true);
            SetInteraction(true);
            
            if (hasSlide) rectTransform.anchoredPosition = showPos;
            if (hasScale) rectTransform.localScale = showScale;
            if (hasFade) canvasGroup.alpha = showAlpha;
        }

        public void HideInstant(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;
        
            StopAllCoroutines();

            gameObject.SetActive(false);
            SetInteraction(false);
            
            if (hasSlide) rectTransform.anchoredPosition = GetHiddenPos(overrideDirection);
            if (hasScale) rectTransform.localScale = hiddenScale;
            if (hasFade) canvasGroup.alpha = hiddenAlpha;
        }

        protected void SetInteraction(bool setTo)
        {
            canvasGroup.interactable = setTo;
            canvasGroup.blocksRaycasts = setTo;
        }

        protected Vector2 GetHiddenPos(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            Vector2 hiddenPos = showPos;

            SlideDirection resolvedDir = data.slideDirection;
            if (resolvedDir == SlideDirection.Auto)
                if (overrideDirection == SlideDirection.Auto)
                    resolvedDir = SlideDirection.Down;
                else
                    resolvedDir = overrideDirection;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float dist = data.slideDistance;

            switch (resolvedDir)
            {
                case SlideDirection.Left: hiddenPos.x -= screenWidth * dist; break;
                case SlideDirection.Right: hiddenPos.x += screenWidth * dist; break;
                case SlideDirection.Up: hiddenPos.y += screenHeight * dist; break;
                case SlideDirection.Down: hiddenPos.y -= screenHeight * dist; break;
            };

            return hiddenPos;
        }
    }
}