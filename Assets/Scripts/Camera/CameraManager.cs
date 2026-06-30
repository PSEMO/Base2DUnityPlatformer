using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            targets = new Dictionary<Transform, float>();
        }

        [SerializeField] CameraSO data;

        private Dictionary<Transform, float> targets;
        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            MoveTowardsTheTarget(GetTargetPos());
        }

        private void MoveTowardsTheTarget(Vector3 targetPos)
        {
            Vector3 nextPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, data.smoothTime, data.maxSpeed);

            if (data.useBounds)
            {
                nextPos.x = Mathf.Clamp(nextPos.x, data.minBounds.x, data.maxBounds.x);
                nextPos.y = Mathf.Clamp(nextPos.y, data.minBounds.y, data.maxBounds.y);
            }

            transform.position = nextPos;
        }

        private Vector3 GetTargetPos()
        {
            if (targets.Count > 0)
            {
                Vector2 endPosition = Vector2.zero;

                foreach (Transform target in targets.Keys)
                {
                    endPosition += (Vector2)(target.position / targets[target]);
                }

                endPosition /= targets.Count;

                return new Vector3 (endPosition.x, endPosition.y, transform.position.z);
            }

            return transform.position;
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
}