using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Environment.Functionality
{
    [RequireComponent(typeof(Collider2D))]
    public class EndGameOnContact : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D _)
        {
            OnContact();
        }

        void OnCollisionEnter2D(Collision2D _)
        {
            OnContact();
        }

        void OnContact()
        {
            UIEvents.InvokeEndGame();
        }
    }
}
