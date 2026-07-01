using System.Collections.Generic;
using UnityEngine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Core.Management
{
    public class Instantiator : MonoBehaviour
    {
        public static Instantiator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        Dictionary<string, Queue<GameObject>> pooledObjects = new();

        public void DeSpawnObject(GameObject obj)
        {
            if (obj.TryGetComponent(out Pooler pooler))
            {
                string id = pooler.GroupName;

                if (!pooledObjects.ContainsKey(id))
                {
                    pooledObjects[id] = new();
                }

                obj.SetActive(false);
                pooledObjects[id].Enqueue(obj);
                pooler.ResetObject();
            }
            else
            {
                Debug.LogWarning("You cannot pool that!");

                Destroy(obj);
            }
        }

        public GameObject SpawnObject(GameObject obj, Vector3 pos, Quaternion rotation, Transform parent = null)
        {
            if (obj.TryGetComponent(out Pooler pooler))
            {
                string id = pooler.GroupName;

                if (pooledObjects.TryGetValue(id, out Queue<GameObject> queue))
                {
                    while (queue.Count > 0)
                    {
                        GameObject spawnedObj = queue.Dequeue();
                        if (spawnedObj == null) continue;

                        spawnedObj.transform.SetPositionAndRotation(pos, rotation);
                        spawnedObj.transform.SetParent(parent);
                        spawnedObj.SetActive(true);
                        return spawnedObj;
                    }
                }
            }

            return Instantiate(obj, pos, rotation, parent);
        }
    }
}