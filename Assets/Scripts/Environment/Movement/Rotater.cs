using PSEMO.Environment.Functionality;
using UnityEngine;

namespace PSEMO.Environment.Movement
{
    public class Rotater : MonoBehaviour, IPoolable
    {
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;
        [SerializeField] private bool unscaledTime = false;

        private Quaternion initialRotation;
        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            if (rb != null) return;

            if (unscaledTime)
            {
                transform.Rotate(rotationAxis, rotationSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            float dt = unscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
            
            rb.MoveRotation(rb.rotation + (rotationSpeed * dt * rotationAxis.z));
        }

        public void ResetObject()
        {
            transform.rotation = initialRotation;
            if (rb != null)
            {
                rb.rotation = initialRotation.eulerAngles.z;
                rb.angularVelocity = 0f;
            }
        }
    }
}