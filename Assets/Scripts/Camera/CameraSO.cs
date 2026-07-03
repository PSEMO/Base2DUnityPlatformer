using UnityEngine;

namespace PSEMO.Camera
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "SO/Camera")]
    public class CameraSO : ScriptableObject
    {
        [Header("Follow Settings")]
        public Vector2 offset = Vector2.zero;
        public float smoothTime = 0.25f;
        public float maxSpeed = Mathf.Infinity;

        [Header("Camera Bounds")]
        public bool useBounds = false;
        public Vector2 minBounds = Vector2.zero;
        public Vector2 maxBounds = Vector2.zero;
    }
}