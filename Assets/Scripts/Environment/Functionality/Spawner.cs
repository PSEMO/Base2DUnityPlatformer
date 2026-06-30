using UnityEngine;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality
{
    public class Spawner : MonoBehaviour
    {
        [Tooltip("Time in seconds between each spawn.")]
        [SerializeField] private  float SpawnInterval = 2f;
    
        [Tooltip("Time in seconds before the first spawn.")]
        [SerializeField] private  float InitialDelay = 0f;

        [Space]
        [SerializeField] private GameObject prefabToSpawn;
    
        private float timer;

        private void Start()
        {
            timer = InitialDelay;
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Spawn();
                timer += SpawnInterval; 
            }
        }

        private void Spawn()
        {
            Instantiator.Instance.SpawnObject(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}
