using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Input
{
    public class DeviceGameObjectToggle : MonoBehaviour
    {
        [Tooltip("The GameObject to enable when a controller is being used.")]
        [SerializeField] private GameObject controllerObject;
        
        [Tooltip("The GameObject to enable when a keyboard/mouse is being used.")]
        [SerializeField] private GameObject keyboardObject;

        private void OnEnable()
        {
            InputEvents.OnControllerStatusChanged += HandleDeviceChange;
            
            bool isControllerInitially = DeviceDetectionManager.IsControllerActive;
            HandleDeviceChange(isControllerInitially);
        }

        private void OnDisable()
        {
            InputEvents.OnControllerStatusChanged -= HandleDeviceChange;
        }

        private void HandleDeviceChange(bool isController)
        {
            controllerObject.SetActive(isController);
            keyboardObject.SetActive(!isController);
        }
    }
}
