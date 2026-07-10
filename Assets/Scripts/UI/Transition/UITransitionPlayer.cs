using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup), typeof(UIAnimator))]
    public class UITransitionPlayer : MonoBehaviour
    {
        [SerializeField] private TransitionSO data;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private UIAnimator animator;

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

            animator = GetComponent<UIAnimator>();
            animator.Init();

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

        public void Play(bool show, Action onComplete, SlideDirection overrideDirection = SlideDirection.Auto, float timeDivider = 1)
        {
            if (show)
            {
                animator.PlayAnim(onComplete,
                    GetHiddenPos(overrideDirection), showPos,
                    hiddenScale, showScale,
                    hiddenAlpha, showAlpha,
                    duration / timeDivider,
                    useSmoothing, hasSlide, hasScale, hasFade,
                    true);
            }
            else
            {
                animator.PlayAnim(onComplete,
                    showPos, GetHiddenPos(overrideDirection),
                    showScale, hiddenScale,
                    showAlpha, hiddenAlpha,
                    duration / timeDivider,
                    useSmoothing, hasSlide, hasScale, hasFade,
                    false);
            }
        }

        public void ApplyInstant(bool show, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            if (show)
            {
                animator.PlayInstant(showPos, showScale, showAlpha, hasSlide, hasScale, hasFade, true);
            }
            else
            {
                animator.PlayInstant(GetHiddenPos(overrideDirection), hiddenScale, hiddenAlpha, hasSlide, hasScale, hasFade, false);
            }
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

        public void PlayCustom(Vector2 targetPos, Vector3 targetScale, float targetAlpha, Action onComplete = null, float timeDivider = 1)
        {
            Vector2 startPos = rectTransform.anchoredPosition;
            Vector3 startScale = rectTransform.localScale;
            float startAlpha = canvasGroup.alpha;

            animator.PlayAnim(onComplete, startPos, targetPos, startScale, targetScale, startAlpha, targetAlpha, duration / timeDivider, useSmoothing, hasSlide, hasScale, hasFade, true);
        }

        public void PlayToPos(Vector2 targetPos, Action onComplete = null, float timeDivider = 1)
        {
            float targetAlpha = canvasGroup.alpha;
            Vector3 targetScale = rectTransform.localScale;
            PlayCustom(targetPos, targetScale, targetAlpha, onComplete, timeDivider);
        }

        public void PlayToTransform(RectTransform target, float timeDivider = 1)
        {
            PlayToPos(target.anchoredPosition, null, timeDivider);
        }

        public void PlayToPosAndShow(Vector2 targetPos, Action onComplete = null, float timeDivider = 1)
        {
            PlayCustom(targetPos, showScale, showAlpha, onComplete, timeDivider);
        }

        public void UpdateShowState()
        {
            showPos = rectTransform.anchoredPosition;
            showScale = rectTransform.localScale;
            showAlpha = canvasGroup.alpha;
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