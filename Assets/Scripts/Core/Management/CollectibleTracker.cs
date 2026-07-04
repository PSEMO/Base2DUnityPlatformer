using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSEMO.Environment.Functionality.Collectible;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Core.Management
{
    public class CollectibleTracker : MonoBehaviour, IPersistable
    {
        [SerializeField] private AllCollectibleSOs allCollectibles;
        public AllCollectibleSOs AllCollectibles => allCollectibles;

        public Dictionary<string, int> CollectedCounts { get; private set; } = new();
        public Dictionary<string, CollectibleSO> GroupData { get; private set; } = new();
    
        private void Awake()
        {
            InitializeCollectibles();
        }

        private void InitializeCollectibles()
        {
            foreach (var collectible in allCollectibles.collectibles)
            {
                if (!GroupData.ContainsKey(collectible.group))
                {
                    GroupData[collectible.group] = collectible;
                }
            }
        }

        private void OnEnable()
        {
            CollectibleEvents.OnCollectibleCollected += HandleCollectibleCollected;
        }

        private void OnDisable()
        {
            CollectibleEvents.OnCollectibleCollected -= HandleCollectibleCollected;
        }

        private void HandleCollectibleCollected(string group)
        {
            if (CollectedCounts.ContainsKey(group))
            {
                CollectedCounts[group]++;
            }
            else
            {
                CollectedCounts[group] = 1;
            }

            CollectibleEvents.InvokeCollectibleCountsUpdated(CollectedCounts, GroupData);
        }

        public int GetCount(string group)
        {
            return CollectedCounts.TryGetValue(group, out int count) ? count : 0;
        }

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            CollectibleTrackerSaveData data = JsonUtility.FromJson<CollectibleTrackerSaveData>(jsonData);
            
            CollectedCounts.Clear();
            for (int i = 0; i < data.keys.Count; i++)
            {
                CollectedCounts[data.keys[i]] = data.values[i];
            }
            
            CollectibleEvents.InvokeCollectibleCountsUpdated(CollectedCounts, GroupData);
        }

        public string SaveData()
        {
            CollectibleTrackerSaveData data = new();
            foreach (var kvp in CollectedCounts)
            {
                data.keys.Add(kvp.Key);
                data.values.Add(kvp.Value);
            }
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}