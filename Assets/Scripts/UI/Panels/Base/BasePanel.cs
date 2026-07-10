using System.Collections;
using UnityEngine;
using System;

namespace PSEMO.UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform), typeof(UITransitionPlayer))]
    public abstract class BasePanel : MonoBehaviour
    {
        [SerializeField] protected TransitionSO data;

        private bool isInit = false;

        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        protected UITransitionPlayer transitionPlayer;

        protected bool isOpen;
        [HideInInspector] public bool IsOpen
        {
            get => isOpen;
        }

        public virtual string DisplayName => gameObject.name;

        public virtual void Init()
        {
            if (isInit)
                return;

            isInit = true;

            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            transitionPlayer = GetComponent<UITransitionPlayer>();

            transitionPlayer.Init();
            transitionPlayer.ApplyInstant(false);
            
            SetInteraction(false);
        }

        public virtual void Show(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = true;

            gameObject.SetActive(true);

            StartTransition(true, () => SetInteraction(true), overrideDirection);
        }

        public virtual void Hide(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;

            if (gameObject.activeInHierarchy)
            {
                StartTransition(false, () => gameObject.SetActive(false), overrideDirection);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected void StartTransition(bool show, Action onComplete, SlideDirection overrideDirection = SlideDirection.Auto)
        {
            SetInteraction(false);
            transitionPlayer.Play(show, onComplete, overrideDirection);
        }

        public void ShowInstant()
        {
            isOpen = true;

            gameObject.SetActive(true);
            SetInteraction(true);
            
            transitionPlayer.ApplyInstant(true);
        }

        public void HideInstant(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;
        
            gameObject.SetActive(false);
            SetInteraction(false);
            
            transitionPlayer.ApplyInstant(false, overrideDirection);
        }

        protected void SetInteraction(bool setTo)
        {
            canvasGroup.interactable = setTo;
            canvasGroup.blocksRaycasts = setTo;
        }
    }
}