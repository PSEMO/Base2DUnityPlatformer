using UnityEngine;
using UnityEngine.SceneManagement;
using PSEMO.Events;
using PSEMO.Player;

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

        void OnContact(Collider2D col)
        {
            col.TryGetComponent(out PlayerController ctx);

            if (ctx != null)
            {
                ctx.triggerResetOnNextSave = true;
            }

            PersistenceEvents.InvokeGameSave();

            if (ctx != null)
            {
                ctx.triggerResetOnNextSave = false;
            }

            SceneManager.LoadScene(SceneToLoadName);
        }
    }
}