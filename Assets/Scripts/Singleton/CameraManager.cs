using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private Dictionary<string, TransformAndDivisor> targets;

    void Update()
    {
        if (targets.Count >= 0)
        {
            Vector2 endPosition = Vector2.zero;

            foreach (TransformAndDivisor target in targets.Values)
            {
                endPosition += target.GetDividedPos();
            }

            endPosition /= targets.Count;

            transform.position = endPosition;
        }
    }

    public void AddTarget(string id, Transform transform, float divisor)
    {
        targets.Add(id, new TransformAndDivisor(transform, divisor));
    }

    public void RemoveTarget(string id)
    {
        targets.Remove(id);
    }
}