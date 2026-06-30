using System.Collections.Generic;
using UnityEngine;
using PSEMO.Core;

namespace PSEMO.Environment.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PathFollower : MonoBehaviour, IPoolable, IMover
    {
        [field: SerializeField] public string ID { get; set; }

        [Space]

        [SerializeField] private float speed = 5f;
        [SerializeField] private float distanceToleranceSqr = 0.01f;

        [SerializeField] private List<Transform> waypoints;
        private List<Vector3> targetPositions;
    
        private int currentWaypointIndex = 0;

        private Vector3 directionalSpeed = Vector3.zero;
        private Vector3 targetPos = Vector3.zero;

        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            targetPositions = new();

            foreach (Transform waypoint in waypoints)
            {
                targetPositions.Add(waypoint.position);
            }

            targetPos = targetPositions[currentWaypointIndex];
        }

        private void Update()
        {
            if (Vector3.SqrMagnitude(transform.position - targetPos) <= distanceToleranceSqr)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % targetPositions.Count;
                targetPos = targetPositions[currentWaypointIndex];
            }
        }

        void FixedUpdate()
        {
            directionalSpeed = speed * (targetPos - transform.position).normalized;
            rb.linearVelocity = directionalSpeed;
        }

        public Vector2 GetVelocity()
        {
            return directionalSpeed;
        }

        public void ResetObject()
        {
            currentWaypointIndex = 0;
            rb.linearVelocity = Vector2.zero;
        }
    }
}