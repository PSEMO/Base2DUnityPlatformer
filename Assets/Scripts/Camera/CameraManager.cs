using System.Collections.Generic;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Camera
{
    public class CameraManager : MonoBehaviour
    {
        private void Awake()
        {
            targets = new Dictionary<Transform, float>();
        }

        private void OnEnable()
        {
            CameraEvents.OnCameraTargetAdded += AddTarget;
            CameraEvents.OnCameraTargetRemoved += RemoveTarget;
            UIEvents.OnLoadingEnd += ResetToTarget;
        }

        private void OnDisable()
        {
            CameraEvents.OnCameraTargetAdded -= AddTarget;
            CameraEvents.OnCameraTargetRemoved -= RemoveTarget;
            UIEvents.OnLoadingEnd -= ResetToTarget;
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
                float totalWeight = 0f;

                foreach (Transform target in targets.Keys)
                {
                    float weight = targets[target];
                    endPosition += (Vector2)target.position * weight;
                    totalWeight += weight;
                }

                endPosition /= totalWeight;
                endPosition += data.offset;

                return new Vector3 (endPosition.x, endPosition.y, transform.position.z);
            }

            return transform.position;
        }

        public void AddTarget(Transform _transform, float weight)
        {
            targets.Add(_transform, weight);
        }

        public void RemoveTarget(Transform _tranform)
        {
            targets.Remove(_tranform);
        }

        private void ResetToTarget()
        {
            velocity = Vector3.zero;
            transform.position = GetTargetPos();
        }
    }
}