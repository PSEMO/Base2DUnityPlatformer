using UnityEngine;
using PSEMO.Events;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality
{
    public class LoadSceneOnContact : MonoBehaviour
    {
        [SerializeField] string SceneToLoadName;

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
            PersistenceEvents.InvokeCreateEmptySceneFile(SceneToLoadName);

            SceneLoader.Load(SceneToLoadName);
        }
    }
}