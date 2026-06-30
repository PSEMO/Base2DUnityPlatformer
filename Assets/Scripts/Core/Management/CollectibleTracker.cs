using System.Collections.Generic;
using UnityEngine;

public class CollectibleTracker : MonoBehaviour
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

    private void Start()
    {
        Events.InvokeCollectibleCountsUpdated(CollectedCounts, GroupData);
    }

    private void OnEnable()
    {
        Events.OnCollectibleCollected += HandleCollectibleCollected;
    }

    private void OnDisable()
    {
        Events.OnCollectibleCollected -= HandleCollectibleCollected;
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

        Events.InvokeCollectibleCountsUpdated(CollectedCounts, GroupData);
    }

    public int GetCount(string group)
    {
        return CollectedCounts.TryGetValue(group, out int count) ? count : 0;
    }
}