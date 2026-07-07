using System.Collections;
using UnityEngine;

namespace PSEMO.UI
{
    public class Panel : BasePanel
    {
        public PanelType Type;

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