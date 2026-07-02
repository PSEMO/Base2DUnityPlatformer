using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Core.Persistence
{
    public abstract class Persists : MonoBehaviour
    {
        [Tooltip("Unique ID for this specific object in the scene.")]
        [ContextMenuItem("Generate New ID", nameof(GenerateId))]
        public string persistenceId = System.Guid.NewGuid().ToString();

        [Tooltip("If true, this object's data will be saved globally and persist " + 
            "across scenes.\nGameObjects that have this enabled should also share " + 
            "the same Id with their other scene counterparts")]
        public bool ShouldSaveGlobally = false;

        public void GenerateId()
        {
            persistenceId = System.Guid.NewGuid().ToString();
        }

        void Start()
        {
            PersistenceEvents.InvokePersistsObjectAdded(this);
        }

        void OnDestroy()
        {
            PersistenceEvents.InvokePersistsObjectRemoved(this);
        }

        public abstract void LoadData(string jsonData);
        public abstract string SaveData();
    }
}