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

    private Dictionary<Transform, float> targets;

    void Start()
    {
        targets = new Dictionary<Transform, float>();
    }

    void Update()
    {
        if (targets.Count > 0)
        {
            Vector2 endPosition = Vector2.zero;

            foreach (Transform target in targets.Keys)
            {
                endPosition += (Vector2)(target.position / targets[target]);
            }

            endPosition /= targets.Count;

            transform.position = new Vector3 (endPosition.x, endPosition.y, -10);
        }
    }

    public void AddTarget(Transform _transform, float divisor)
    {
        targets.Add(_transform, divisor);
    }

    public void RemoveTarget(Transform _tranform)
    {
        targets.Remove(_tranform);
    }
}