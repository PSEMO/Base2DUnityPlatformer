using System;
using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(UIAnimator), typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class BaseTransitionPlayer : MonoBehaviour
    {
        [SerializeField] protected TransitionSO data;
        
        protected RectTransform rectTransform;
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

        protected bool isInit = false;

        protected virtual void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            if (isInit)
                return;
            else
                isInit = true; 

            animator = GetComponent<UIAnimator>();
            animator.Init();

            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

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

        public Vector2 GetHiddenPos(Vector2 anchorPos, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            Vector2 hiddenPos = anchorPos;

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