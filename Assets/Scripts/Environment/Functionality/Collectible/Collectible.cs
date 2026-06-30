using UnityEngine;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality.Collectible
{

    public class Collectible : MonoBehaviour
    {
        [SerializeField] CollectibleSO data;

        void OnTriggerEnter2D(Collider2D _)
        {
            HandleContact();
        }
        void OnCollisionEnter2D(Collision2D _)
        {
            HandleContact();
        }

        void HandleContact()
        {
            Events.InvokeCollectibleCollected(data.group);
            Destroy(gameObject);
        }
    }
}