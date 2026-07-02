using UnityEngine;
using PSEMO.Player;

namespace PSEMO.Environment.Functionality.Enabler
{
    [RequireComponent(typeof(Collider2D))]
    public class EnableRunOnContact : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            OnContact(col);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            OnContact(col.collider);
        }

        void OnContact(Collider2D col)
        {
            if (col.TryGetComponent(out PlayerController playerController))
            {
                playerController.EnableRun();
                Destroy(gameObject);
            }
        }
    }
}