using PSEMO.Environment.Functionality;
using UnityEngine;
using PSEMO.Core.Persistence;

namespace PSEMO.Environment.Movement
{
    public class Rotater : MonoBehaviour, IPoolable, IPersistable
    {
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;
        [SerializeField] private bool unscaledTime = false;
        [SerializeField] private bool useGlobalTime = false;

        private Quaternion initialRotation;
        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            initialRotation = transform.rotation;
        }

        void Start()
        {
            
        }

        private void Update()
        {
            if (rb != null) return;

            if (useGlobalTime)
            {
                float time = unscaledTime ? Time.unscaledTime : Time.time;
                transform.rotation = initialRotation * Quaternion.AngleAxis(rotationSpeed * time, rotationAxis);
            }
            else
            {
                if (unscaledTime)
                {
                    transform.Rotate(rotationAxis, rotationSpeed * Time.unscaledDeltaTime);
                }
                else
                {
                    transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            if (useGlobalTime)
            {
                float time = unscaledTime ? Time.unscaledTime : Time.time;
                rb.MoveRotation(initialRotation.eulerAngles.z + (rotationSpeed * time * rotationAxis.z));
            }
            else
            {
                float dt = unscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                rb.MoveRotation(rb.rotation + (rotationSpeed * dt * rotationAxis.z));
            }
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

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            RotaterSaveData data = JsonUtility.FromJson<RotaterSaveData>(jsonData);
            
            transform.rotation = data.rotation;
            if (rb != null)
            {
                rb.rotation = data.rotation.eulerAngles.z;
            }
        }

        public string SaveData()
        {
            RotaterSaveData data = new()
            {
                rotation = transform.rotation
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}