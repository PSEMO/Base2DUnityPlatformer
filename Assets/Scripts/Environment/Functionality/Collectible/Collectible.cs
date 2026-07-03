using PSEMO.Audio;
using UnityEngine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Environment.Functionality.Collectible
{

    public class Collectible : MonoBehaviour, IPersistable
    {
        [SerializeField] CollectibleSO data;

        [HideInInspector] public bool isCollected = false;

        void OnTriggerEnter2D(Collider2D _)
        {
            HandleContact();
        }
        void OnCollisionEnter2D(Collision2D _)
        {
            HandleContact();
        }

        private void HandleContact()
        {
            AudioManager.Instance.PlayAudio(AudioConstants.Coin);
            
            isCollected = true;
            CollectibleEvents.InvokeCollectibleCollected(data.group);
            gameObject.SetActive(false);
        }

        private void SetAsCollected()
        {
            isCollected = true;
            gameObject.SetActive(false);
        }

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            CollectibleSaveData data = JsonUtility.FromJson<CollectibleSaveData>(jsonData);
            
            isCollected = data.isCollected;
            if (isCollected)
            {
                SetAsCollected();
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