using System;
using UnityEngine;

namespace PSEMO.Events
{
    public static class CameraEvents
    {
        public static event Action<Transform, float> OnCameraTargetAdded;
        public static void InvokeCameraTargetAdded(Transform target, float weight) => OnCameraTargetAdded?.Invoke(target, weight);

        public static event Action<Transform> OnCameraTargetRemoved;
        public static void InvokeCameraTargetRemoved(Transform target) => OnCameraTargetRemoved?.Invoke(target);
    }
}