using UnityEngine;

namespace PSEMO.UI
{
    public class SubPanel : BasePanel
    {
        [SerializeField] string subPanelDisplayName;

        public string DisplayName => subPanelDisplayName;

        public override void Show(SlideDirection overrideDirection = SlideDirection.Auto)
        {
            isOpen = true;

            gameObject.SetActive(true);

            StartTransition(true, () => SetInteraction(true), overrideDirection);
        }

        public override void Hide(SlideDirection overrideDirection = SlideDirection.Auto)
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
    }
}