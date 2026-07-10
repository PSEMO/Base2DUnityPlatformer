using System.Collections;
using UnityEngine;
using System;

namespace PSEMO.UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform), typeof(UITransitionPlayer))]
    public abstract class BasePanel : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        protected UITransitionPlayer transitionPlayer;

        protected bool isOpen;
        [HideInInspector] public bool IsOpen
        {
            get => isOpen;
        }

        public virtual string DisplayName => gameObject.name;

        private bool isInit = false;

        void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            if (isInit)
                return;
            else
                isInit = true;

            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            transitionPlayer = GetComponent<UITransitionPlayer>();

            transitionPlayer.Init();
            transitionPlayer.ApplyInstant(false);
        }

        public virtual void Show(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = true;

            StartTransition(true, null, overrideDirection);
        }

        public virtual void Hide(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;

            if (gameObject.activeInHierarchy)
            {
                StartTransition(false, null, overrideDirection);
            }
            else
            {
                transitionPlayer.ApplyInstant(false, overrideDirection);
            }
        }

        protected void StartTransition(bool show, Action onComplete, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            transitionPlayer.Play(show, onComplete, overrideDirection);
        }

        public void ShowInstant()
        {
            isOpen = true;

            transitionPlayer.ApplyInstant(true);
        }

        public void HideInstant(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;
        
            transitionPlayer.ApplyInstant(false, overrideDirection);
        }
    }
}