using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Input
{
    public class UIGameObjectToggle : MonoBehaviour
    {
        [Tooltip("The GameObject to enable when a menu is open, and disable when closed.")]
        [SerializeField] private GameObject targetObject;

        [Tooltip("Check this if the scene starts with a menu open (e.g., Main Menu).")]
        [SerializeField] private bool startsWithUIOpen = false;

        private void OnEnable()
        {
            UIEvents.OnMenuVisibilityChanged += HandleVisibilityChanged;
            
            HandleVisibilityChanged(startsWithUIOpen);
        }

        private void OnDisable()
        {
            UIEvents.OnMenuVisibilityChanged -= HandleVisibilityChanged;
        }

        private void HandleVisibilityChanged(bool isVisible)
        {
            targetObject.SetActive(isVisible);
        }
    }
}
