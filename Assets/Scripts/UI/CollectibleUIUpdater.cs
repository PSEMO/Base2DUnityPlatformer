using UnityEngine;
using TMPro;
using System.Collections.Generic;
using PSEMO.Environment.Functionality.Collectible;
using PSEMO.Events;
using PSEMO.Core.Management;

namespace PSEMO.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CollectibleUIUpdater : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private CollectibleTracker tracker;

        private void OnEnable()
        {
            CollectibleEvents.OnCollectibleCountsUpdated += UpdateUI;

            if (tracker == null)
                tracker = FindFirstObjectByType<CollectibleTracker>();
            
            if (tracker != null)
                UpdateUI(tracker.CollectedCounts, tracker.GroupData);
        }

        private void OnDisable()
        {
            CollectibleEvents.OnCollectibleCountsUpdated -= UpdateUI;
        }

        private void UpdateUI(Dictionary<string, int> collectedCounts, Dictionary<string, CollectibleSO> groupData)
        {
            if (groupData.Count <= 0)
            {
                textMeshPro.text = "";
                return;
            }
        
            int allCurrent = 0;
            int allMax = 0;
        
            string outputText = "";
            int currentIndex = 0;
        
            foreach (var kvp in groupData)
            {
                string group = kvp.Key;
                var collectible = kvp.Value;

                string displayName = collectible.displayName;
                int maxCount = collectible.totalAmountOfThisGroup;
                int currentCount = collectedCounts.TryGetValue(group, out int count) ? count : 0;
            
                allCurrent += currentCount;
                allMax += maxCount;

                outputText += $"{displayName}: {currentCount}/{maxCount}";
            
                if (currentIndex < groupData.Count - 1)
                {
                    outputText += ", ";
                }
                currentIndex++;
            }

            string prefix = $"All: {allCurrent}/{allMax}, ";
            textMeshPro.text = prefix + outputText;
        }
    }
}