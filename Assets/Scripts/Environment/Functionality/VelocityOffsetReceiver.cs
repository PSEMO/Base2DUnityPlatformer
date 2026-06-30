using System.Collections.Generic;
using UnityEngine;
using PSEMO.Environment.Movement;

namespace PSEMO.Environment.Functionality
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VelocityOffsetReceiver : MonoBehaviour, IVelocityOffsettable
    {
        private Rigidbody2D rb;
        private List<IMover> additionalVelocities = new();

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (additionalVelocities.Count > 0)
            {
                Vector2 totalExtraVelocity = Vector2.zero;
                foreach (IMover vel in additionalVelocities)
                {
                    totalExtraVelocity += vel.GetVelocity();
                }
                rb.linearVelocity += totalExtraVelocity;
            }
        }

        public void AddVelocityOffset(IMover source)
        {
            additionalVelocities.Add(source);
        }

        public void RemoveVelocityOffset(IMover source)
        {
            additionalVelocities.Remove(source);
        }
    }
}