using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Environment.Functionality
{
    [RequireComponent(typeof(Collider2D))]
    public class EndGameOnContact : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            OnContact(col);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            OnContact(col.collider);
        }

        void OnContact(Collider2D _)
        {
            UIEvents.InvokeEndGame();
        }
    }
}
