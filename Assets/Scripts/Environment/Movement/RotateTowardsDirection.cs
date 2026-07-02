using UnityEngine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Environment.Movement
{
    [RequireComponent(typeof(IMover))]
    public class RotateTowardsDirection : MonoBehaviour, IPoolable
    {
        private IMover mover;

        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private float minVelocityThreshold = 0.01f;
        [SerializeField] private float angleOffset = 0f;

        private Quaternion initialRotation;
        private Vector3 initialScale;
        private float facing = 1f;

        private void Awake()
        {
            mover = GetComponent<IMover>();
        }

        private void Start()
        {
            initialRotation = transform.rotation;
            initialScale = transform.localScale;
        }

        private void Update()
        {
            Vector2 velocity = mover.GetVelocity();
            if (velocity.sqrMagnitude > minVelocityThreshold * minVelocityThreshold)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + angleOffset;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                if (rotationSpeed > 0f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = targetRotation;
                }

                if (Mathf.Abs(velocity.x) > 0.01f)
                {
                    facing = velocity.x > 0 ? 1f : -1f;
                }

                transform.localScale = new Vector3(
                    initialScale.x,
                    initialScale.y * facing,
                    initialScale.z);
            }
        }

        public void ResetObject()
        {
            transform.rotation = initialRotation;
            transform.localScale = initialScale;
            facing = 1f;
        }
    }
}