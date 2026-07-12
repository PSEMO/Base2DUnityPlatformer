using System;
using UnityEngine;

namespace PSEMO.Events
{
    public static class InputEvents
    {
        public static event Action<bool> OnControllerStatusChanged;
        public static void InvokeControllerStatusChanged(bool isController) => OnControllerStatusChanged?.Invoke(isController);
    }
}
