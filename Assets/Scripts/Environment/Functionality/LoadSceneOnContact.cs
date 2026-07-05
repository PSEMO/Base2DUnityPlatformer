using UnityEngine;
using PSEMO.Events;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality
{
    public class LoadSceneOnContact : MonoBehaviour
    {
        [SerializeField] string SceneToLoadName;

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
            PersistenceEvents.InvokeCreateEmptySceneFile(SceneToLoadName);

            SceneLoader.Load(SceneToLoadName);
        }
    }
}