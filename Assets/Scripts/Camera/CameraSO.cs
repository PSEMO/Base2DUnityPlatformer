using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "SO/Camera")]
public class CameraSO : ScriptableObject
{
    [Header("Follow Settings")]
    public float smoothTime = 0.25f;
    public float maxSpeed = Mathf.Infinity;

    [Header("Camera Bounds")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;
}