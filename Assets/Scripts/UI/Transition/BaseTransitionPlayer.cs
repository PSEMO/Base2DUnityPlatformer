using System;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup), typeof(UIAnimator))]
    public class BaseTransitionPlayer : MonoBehaviour
    {
        [SerializeField] private TransitionSO data;
        
        [HideInInspector] public RectTransform rectTransform;
        protected CanvasGroup canvasGroup;

        protected UIAnimator animator;

        protected bool hasFade;
        protected bool hasSlide;
        protected bool hasScale;
        protected bool useSmoothing;
        
        protected float duration;
        
        protected Vector2 hiddenScale;
        protected float hiddenAlpha;
        
        protected Vector2 showPos;
        protected Vector3 showScale;
        protected float showAlpha;

        private bool isInit = false;

        void Awake()
        {
            Init();
        }

        public virtual void Init()
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
    }
}