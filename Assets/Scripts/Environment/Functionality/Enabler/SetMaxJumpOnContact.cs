using UnityEngine;
using PSEMO.Player;
using PSEMO.Core.Persistence;

namespace PSEMO.Environment.Functionality.Enabler
{
    [RequireComponent(typeof(Collider2D))]
    public class SetMaxJumpOnContact : MonoBehaviour, IPersistable
    {
        [SerializeField] int newCount;
        
        [HideInInspector] public bool isCollected = false;

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
                playerController.SetMaxJumpCount(newCount);
                isCollected = true;
                gameObject.SetActive(false);
            }
        }

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            CollectibleSaveData data = JsonUtility.FromJson<CollectibleSaveData>(jsonData);
            
            isCollected = data.isCollected;
            if (isCollected)
            {
                gameObject.SetActive(false);
            }
        }

        public string SaveData()
        {
            CollectibleSaveData data = new()
            {
                isCollected = isCollected
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}