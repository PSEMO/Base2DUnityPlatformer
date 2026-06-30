using System.Collections;
using UnityEngine;

namespace PSEMO.Environment.Functionality.Enabler
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnableOnContact : MonoBehaviour
    {
        [Header("Object to enable")]
        [SerializeField] private GameObject objectToEnable;

        [Header("How long should the object be enabled for? (0 or less is infinite)")]
        [SerializeField] private float duration;

        private Coroutine EnablingRoutine = null;

        void OnTriggerEnter2D(Collider2D _)
        {
            HandleEnter();
        }

        void OnCollisionEnter2D(Collision2D _)
        {
            HandleEnter();
        }

        void HandleEnter()
        {
            if (duration <= 0)
            {
                objectToEnable.SetActive(true);
            }
            else
            {
                if (EnablingRoutine != null)
                    StopCoroutine(EnablingRoutine);
            
                EnablingRoutine = StartCoroutine(EnableObject(duration));
            }
        }

        IEnumerator EnableObject(float duration)
        {
            objectToEnable.SetActive(true);

            yield return new WaitForSeconds(duration);

            objectToEnable.SetActive(false);
        }
    }
}