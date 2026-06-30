using UnityEngine;

namespace PSEMO.Environment.Functionality.Enabler
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnableWhileContact : MonoBehaviour
    {
        [Header("Object to enable")]
        [SerializeField] private GameObject objectToEnable;

        void OnTriggerEnter2D(Collider2D _)
        {
            HandleEnter();
        }
        void OnCollisionEnter2D(Collision2D _)
        {
            HandleEnter();
        }

        void OnTriggerExit2D(Collider2D _)
        {
            HandleExit();
        }
        void OnCollisionExit2D(Collision2D _)
        {
            HandleExit();
        }

        void HandleEnter()
        {
            objectToEnable.SetActive(true);
        }

        void HandleExit()
        {
            objectToEnable.SetActive(false);
        }
    }
}