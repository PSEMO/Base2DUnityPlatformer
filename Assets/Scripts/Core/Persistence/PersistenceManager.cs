using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using PSEMO.Events;
using System.Collections;

namespace PSEMO.Core.Persistence
{
    public class PersistenceManager : MonoBehaviour
    {
        private static string GameNameSuffix => $".data.{Application.productName}";
        
        private static string GetGlobalFilePath() => Path.Combine(Application.persistentDataPath, "Global", $"Global{GameNameSuffix}");
        private static string GetSceneFilePath(string sceneName) => Path.Combine(Application.persistentDataPath, $"{sceneName}{GameNameSuffix}");

        private static string[] GetAllSceneFiles() => Directory.GetFiles(Application.persistentDataPath, $"*{GameNameSuffix}");

        private List<Persists> dataPersistenceObjects;

        void Awake()
        {
            dataPersistenceObjects = new();
        }

        void Start()
        {
            StartCoroutine(LateStart());
        }

        IEnumerator LateStart()
        {
            yield return null;

            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        void OnEnable()
        {
            PersistenceEvents.OnGameSave += SaveTheGame;
            PersistenceEvents.OnGameSaveDelete += DeleteGameData;
            PersistenceEvents.OnPersistsObjectAdded += AddPersistentObj;
            PersistenceEvents.OnPersistsObjectRemoved += RemovePersistentObj;
        }

        void OnDisable()
        {
            PersistenceEvents.OnGameSave -= SaveTheGame;
            PersistenceEvents.OnGameSaveDelete -= DeleteGameData;
            PersistenceEvents.OnPersistsObjectAdded -= AddPersistentObj;
            PersistenceEvents.OnPersistsObjectRemoved -= RemovePersistentObj;
        }

        void AddPersistentObj(Persists objToAdd)
        {
            dataPersistenceObjects.Add(objToAdd);
        }

        void RemovePersistentObj(Persists objToRemove)
        {
            dataPersistenceObjects.Remove(objToRemove);
        }

        void LoadGame()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SerializableDictionary globalDict = LoadFromFile(GetGlobalFilePath());
            SerializableDictionary sceneDict = LoadFromFile(GetSceneFilePath(sceneName));

            Dictionary<string, string> globalData = new();
            if (globalDict != null)
            {
                for (int i = 0; i < globalDict.keys.Count; i++)
                {
                    globalData[globalDict.keys[i]] = globalDict.values[i];
                }
            }

            Dictionary<string, string> sceneData = new();
            if (sceneDict != null)
            {
                for (int i = 0; i < sceneDict.keys.Count; i++)
                {
                    sceneData[sceneDict.keys[i]] = sceneDict.values[i];
                }
            }

            foreach (Persists dataPersistenceObj in dataPersistenceObjects)
            {
                bool isGlobal = dataPersistenceObj.ShouldSaveGlobally;

                Dictionary<string, string> targetData = isGlobal ? globalData : sceneData;

                if (targetData.TryGetValue(dataPersistenceObj.persistenceId, out string jsonData))
                {
                    dataPersistenceObj.LoadData(jsonData);
                }
            }
        }

        void DeleteGameData()
        {
            string globalPath = GetGlobalFilePath();
            if (File.Exists(globalPath))
            {
                File.Delete(globalPath);
                Debug.Log("Global game data deleted.");
            }

            string[] files = GetAllSceneFiles();
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log($"Scene game data deleted: {file}");
            }
        }

        void SaveTheGame()
        {
            SerializableDictionary globalDictToSave = LoadFromFile(GetGlobalFilePath());
            globalDictToSave ??= new SerializableDictionary();

            SerializableDictionary sceneDictToSave = new();
            HashSet<string> processedGlobalIds = new();

            foreach (Persists dataPersistenceObj in dataPersistenceObjects)
            {
                bool isGlobal = dataPersistenceObj.ShouldSaveGlobally;

                if (isGlobal)
                {
                    if (processedGlobalIds.Contains(dataPersistenceObj.persistenceId))
                    {
                        Debug.LogWarning($"Duplicate Global ID found:");
                        Debug.LogWarning($"{dataPersistenceObj.persistenceId}");
                        Debug.LogWarning($"{dataPersistenceObj.gameObject.name}");
                        Debug.LogWarning("------------------------------------");
                        continue;
                    }
                    processedGlobalIds.Add(dataPersistenceObj.persistenceId);

                    int index = globalDictToSave.keys.IndexOf(dataPersistenceObj.persistenceId);
                    if (index >= 0)
                    {
                        globalDictToSave.values[index] = dataPersistenceObj.SaveData();
                    }
                    else
                    {
                        globalDictToSave.keys.Add(dataPersistenceObj.persistenceId);
                        globalDictToSave.values.Add(dataPersistenceObj.SaveData());
                    }
                }
                else
                {
                    if (sceneDictToSave.keys.Contains(dataPersistenceObj.persistenceId))
                    {
                        Debug.LogWarning($"Duplicate Scene ID found:");
                        Debug.LogWarning($"{dataPersistenceObj.persistenceId}");
                        Debug.LogWarning($"{dataPersistenceObj.gameObject.name}");
                        Debug.LogWarning("------------------------------------");
                        continue;
                    }
                    sceneDictToSave.keys.Add(dataPersistenceObj.persistenceId);
                    sceneDictToSave.values.Add(dataPersistenceObj.SaveData());
                }
            }

            SaveToFile(GetGlobalFilePath(), globalDictToSave);
            
            string sceneName = SceneManager.GetActiveScene().name;
            SaveToFile(GetSceneFilePath(sceneName), sceneDictToSave);
        }

        List<Persists> FindAllDataPersistenceObjects()
        {
            IEnumerable<Persists> dataPersistenceObjects = FindObjectsByType<Persists>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return new List<Persists>(dataPersistenceObjects);
        }

        SerializableDictionary LoadFromFile(string fullPath) 
        {
            SerializableDictionary loadedData = null;
            if (File.Exists(fullPath)) 
            {
                try 
                {
                    string dataToLoad = File.ReadAllText(fullPath);
                    loadedData = JsonUtility.FromJson<SerializableDictionary>(dataToLoad);
                }
                catch (System.Exception e) 
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }
            return loadedData;
        }

        void SaveToFile(string fullPath, SerializableDictionary data) 
        {
            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data, true);
                File.WriteAllText(fullPath, dataToStore);
            }
            catch (System.Exception e) 
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public static bool HasSceneData(string sceneName = "")
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                string[] files = GetAllSceneFiles();
                return files.Length > 0;
            }
            else
            {
                string fullPath = GetSceneFilePath(sceneName);
                return File.Exists(fullPath);
            }
        }
    
        public static int FurthestAvailableSceneIndex()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = sceneCount - 1; i >= 0; i--)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                
                if (!string.IsNullOrEmpty(sceneName) && HasSceneData(sceneName))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}