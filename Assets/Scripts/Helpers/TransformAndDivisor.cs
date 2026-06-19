using UnityEngine;

public class TransformAndDivisor
{
    public Transform transform;
    public float divisor;

    public TransformAndDivisor(Transform _transform, float _divisor)
    {
        transform = _transform;
        divisor = _divisor;
    }

    public Vector2 GetDividedPos()
    {
        return transform.position / divisor;
    }
}