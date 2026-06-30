using UnityEngine;
using TMPro;
using System.Collections.Generic;
using PSEMO.Core.Management;
using PSEMO.Environment.Functionality.Collectible;

namespace PSEMO.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CollectibleUIUpdater : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponentInParent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Events.OnCollectibleCountsUpdated += UpdateUI;
        }

        private void OnDisable()
        {
            Events.OnCollectibleCountsUpdated -= UpdateUI;
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