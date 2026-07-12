using UnityEngine;
using UnityEngine.InputSystem;
using PSEMO.Events;

namespace PSEMO.InputSettings
{
    public static class DeviceDetectionManager
    {
        public static bool IsControllerActive { get; private set; } = false;
        private static bool hasInputOccurred = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            hasInputOccurred = false;
            IsControllerActive = false;

            if (Gamepad.current != null)
            {
                IsControllerActive = true;
                InputEvents.InvokeControllerStatusChanged(true);
            }
            else
            {
                IsControllerActive = false;
                InputEvents.InvokeControllerStatusChanged(false);
            }
            
            InputSystem.onActionChange += OnActionChange;
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added && !hasInputOccurred)
            {
                if (device is Gamepad || device is Joystick)
                {
                    IsControllerActive = true;
                    InputEvents.InvokeControllerStatusChanged(true);
                }
            }
            else if (change == InputDeviceChange.Removed && IsControllerActive && device is Gamepad)
            {
                if (Gamepad.all.Count == 0)
                {
                    IsControllerActive = false;
                    InputEvents.InvokeControllerStatusChanged(false);
                }
            }
        }

        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var action = (InputAction)obj;
                var control = action.activeControl;
                if (control != null)
                {
                    hasInputOccurred = true;
                    var device = control.device;
                    
                    bool isGamepad = device is Gamepad || device is Joystick;
                    
                    if (isGamepad && !IsControllerActive)
                    {
                        IsControllerActive = true;
                        InputEvents.InvokeControllerStatusChanged(true);
                    }
                    else if (!isGamepad && IsControllerActive)
                    {
                        IsControllerActive = false;
                        InputEvents.InvokeControllerStatusChanged(false);
                    }
                }
            }
        }
    }
}
