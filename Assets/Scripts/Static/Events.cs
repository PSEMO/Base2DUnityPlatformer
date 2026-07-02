using System;
using System.Collections.Generic;
using UnityEngine;
using PSEMO.Environment.Functionality.Collectible;

public static class Events
{
    public static event Action OnPlayerDeath;
    public static void InvokePlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action<Vector3> OnCheckPointReached;
    public static void InvokeCheckPointReached(Vector3 newSpawnPos) => OnCheckPointReached?.Invoke(newSpawnPos);

    public static event Action OnGameSave;
    public static void InvokeGameSave() => OnGameSave?.Invoke();

    public static event Action OnGameSaveDelete;
    public static void InvokeGameSaveDelete() => OnGameSaveDelete?.Invoke();

    public static event Action<string> OnCollectibleCollected;
    public static void InvokeCollectibleCollected(string group) => OnCollectibleCollected?.Invoke(group);

    public static event Action<Dictionary<string, int>, Dictionary<string, CollectibleSO>> OnCollectibleCountsUpdated;
    public static void InvokeCollectibleCountsUpdated(Dictionary<string, int> counts, Dictionary<string, CollectibleSO> groupData) => OnCollectibleCountsUpdated?.Invoke(counts, groupData);

    public static event Func<GameObject, Vector3, Quaternion, Transform, GameObject> OnSpawnObject;
    public static GameObject InvokeSpawnObject(GameObject obj, Vector3 pos, Quaternion rotation, Transform parent = null)
    {
        if (OnSpawnObject != null)
            return OnSpawnObject.Invoke(obj, pos, rotation, parent);
        else
            return UnityEngine.Object.Instantiate(obj, pos, rotation, parent);
    }

    public static event Action<GameObject> OnDeSpawnObject;
    public static void InvokeDeSpawnObject(GameObject obj)
    {
        if (OnDeSpawnObject == null)
            UnityEngine.Object.Destroy(obj);
        else
            OnDeSpawnObject.Invoke(obj);
    }
}