using UnityEngine;

namespace PSEMO.UI
{
    [RequireComponent(typeof(PanelTransitionPlayer))]
    public abstract class BasePanel : MonoBehaviour
    {
        private bool isInit = false;

        protected PanelTransitionPlayer transitionPlayer;

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
            else
                isInit = true;
            
            transitionPlayer = GetComponent<PanelTransitionPlayer>();

            transitionPlayer.Init();
            transitionPlayer.PlayInstant(false);
        }

        public virtual void Show(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = true;

            transitionPlayer.Play(true, null, overrideDirection);
        }

        public virtual void Hide(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = false;

            transitionPlayer.Play(false, null, overrideDirection);
        }

        public void ShowInstant()
        {
            isOpen = true;
            
            transitionPlayer.PlayInstant(true);
        }

        public void HideInstant()
        {
            isOpen = false;
            
            transitionPlayer.PlayInstant(false);
        }
    }
}