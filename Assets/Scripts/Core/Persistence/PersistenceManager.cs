using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using PSEMO.Events;
using System.Threading.Tasks;
using System;

namespace PSEMO.Core.Persistence
{
    public class PersistenceManager : MonoBehaviour
    {
        private static string GameNameSuffix => $".data.{Application.productName}";
        
        private static string _currentSaveSlot;
        public static string CurrentSaveSlot
        {
            get
            {
                if (string.IsNullOrEmpty(_currentSaveSlot))
                {
                    _currentSaveSlot = PlayerPrefs.GetString("LastSaveSlot", "SaveSlot1");
                }
                return _currentSaveSlot;
            }
            private set
            {
                _currentSaveSlot = value;
                PlayerPrefs.SetString("LastSaveSlot", value);
                PlayerPrefs.Save();
            }
        }
        private static string CurrentSaveSlotPath => Path.Combine(Application.persistentDataPath, CurrentSaveSlot);
        
        private static string GetGlobalFilePath() => Path.Combine(CurrentSaveSlotPath, "Global", $"Global{GameNameSuffix}");
        private static string GetSceneFilePath(string sceneName) => Path.Combine(CurrentSaveSlotPath, $"{sceneName}{GameNameSuffix}");

        private static string[] GetAllSceneFiles()
        {
            if (!Directory.Exists(CurrentSaveSlotPath))
            {
                return new string[0];
            }
            return Directory.GetFiles(CurrentSaveSlotPath, $"*{GameNameSuffix}");
        }

        private List<Persister> dataPersistenceObjects;

        void Awake()
        {
            dataPersistenceObjects = new();
        }

        void Start()
        {
            LoadGame();
        }

        void OnEnable()
        {
            PersistenceEvents.OnGameSave += SaveTheGame;
            PersistenceEvents.OnGameSaveDelete += DeleteGameData;
            PersistenceEvents.OnSceneSaveDelete += DeleteSceneData;
            PersistenceEvents.OnCreateEmptySceneFile += CreateEmptySceneFile;
            PersistenceEvents.OnPersistsObjectAdded += AddPersistentObj;
            PersistenceEvents.OnPersistsObjectRemoved += RemovePersistentObj;
            PersistenceEvents.OnSaveSlotChanged += ChangeSaveSlot;
        }

        void OnDisable()
        {
            PersistenceEvents.OnGameSave -= SaveTheGame;
            PersistenceEvents.OnGameSaveDelete -= DeleteGameData;
            PersistenceEvents.OnSceneSaveDelete -= DeleteSceneData;
            PersistenceEvents.OnCreateEmptySceneFile -= CreateEmptySceneFile;
            PersistenceEvents.OnPersistsObjectAdded -= AddPersistentObj;
            PersistenceEvents.OnPersistsObjectRemoved -= RemovePersistentObj;
            PersistenceEvents.OnSaveSlotChanged -= ChangeSaveSlot;
        }

        void AddPersistentObj(Persister objToAdd)
        {
            dataPersistenceObjects.Add(objToAdd);
        }

        void RemovePersistentObj(Persister objToRemove)
        {
            dataPersistenceObjects.Remove(objToRemove);
        }

        void ChangeSaveSlot(string slotName)
        {
            CurrentSaveSlot = slotName;
        }

        async void LoadGame()
        {
            UIEvents.InvokeLoadingStart();
            try
            {
                string sceneName = SceneManager.GetActiveScene().name;
                string globalPath = GetGlobalFilePath();
                string scenePath = GetSceneFilePath(sceneName);

                var (globalData, sceneData) = await RunTask(() => 
                {
                    var globalDict = LoadFromFile(globalPath);
                    var sceneDict = LoadFromFile(scenePath);

                    Dictionary<string, string> gd = new();
                    if (globalDict != null)
                    {
                        for (int i = 0; i < globalDict.keys.Count; i++)
                        {
                            gd[globalDict.keys[i]] = globalDict.values[i];
                        }
                    }

                    Dictionary<string, string> sd = new();
                    if (sceneDict != null)
                    {
                        for (int i = 0; i < sceneDict.keys.Count; i++)
                        {
                            sd[sceneDict.keys[i]] = sceneDict.values[i];
                        }
                    }

                    return (gd, sd);
                });

                foreach (Persister dataPersistenceObj in dataPersistenceObjects)
                {
                    if (dataPersistenceObj == null) continue;

                    bool isGlobal = dataPersistenceObj.ShouldSaveGlobally;

                    Dictionary<string, string> targetData = isGlobal ? globalData : sceneData;

                    if (targetData.TryGetValue(dataPersistenceObj.persistenceId, out string jsonData))
                    {
                        dataPersistenceObj.LoadData(jsonData);
                    }
                }
            }
            finally
            {
                UIEvents.InvokeLoadingEnd();
            }
        }

        async void DeleteGameData()
        {
            UIEvents.InvokeLoadingStart();
            try
            {
                string globalPath = GetGlobalFilePath();
                string[] files = GetAllSceneFiles();

                await RunTask(() => 
                {
                    if (File.Exists(globalPath))
                    {
                        File.Delete(globalPath);
                        Debug.Log("Global game data deleted.");
                    }

                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Debug.Log($"Scene game data deleted: {file}");
                    }
                });
            }
            finally
            {
                UIEvents.InvokeLoadingEnd();
            }
        }

        async void DeleteSceneData(int sceneIndex)
        {
            UIEvents.InvokeLoadingStart();
            try
            {
                string scenePathName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                string sceneName = Path.GetFileNameWithoutExtension(scenePathName);
                string scenePath = GetSceneFilePath(sceneName);

                await RunTask(() => 
                {
                    if (File.Exists(scenePath))
                    {
                        File.Delete(scenePath);
                        Debug.Log($"Scene game data deleted: {scenePath}");
                    }
                });
            }
            finally
            {
                UIEvents.InvokeLoadingEnd();
            }
        }

        async void CreateEmptySceneFile(string sceneName)
        {
            UIEvents.InvokeLoadingStart();
            try
            {
                string fullPath = GetSceneFilePath(sceneName);

                await RunTask(() => 
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                        File.WriteAllText(fullPath, string.Empty);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error occured when trying to create empty file: " + fullPath + "\n" + e);
                    }
                });
            }
            finally
            {
                UIEvents.InvokeLoadingEnd();
            }
        }

        async void SaveTheGame()
        {
            UIEvents.InvokeLoadingStart();
            try
            {
                string globalPath = GetGlobalFilePath();
                string sceneName = SceneManager.GetActiveScene().name;
                string scenePath = GetSceneFilePath(sceneName);

                var capturedData = new List<(string id, bool isGlobal, string data, string name)>();
                foreach (Persister dataPersistenceObj in dataPersistenceObjects)
                {
                    if (dataPersistenceObj == null) continue;

                    capturedData.Add((
                        dataPersistenceObj.persistenceId, 
                        dataPersistenceObj.ShouldSaveGlobally, 
                        dataPersistenceObj.SaveData(),
                        dataPersistenceObj.gameObject.name
                    ));
                }

                await RunTask(() => 
                {
                    SerializableDictionary globalDictToSave = LoadFromFile(globalPath);
                    globalDictToSave ??= new SerializableDictionary();

                    SerializableDictionary sceneDictToSave = new();
                    HashSet<string> processedGlobalIds = new();

                    foreach (var captured in capturedData)
                    {
                        if (captured.isGlobal)
                        {
                            if (processedGlobalIds.Contains(captured.id))
                            {
                                Debug.LogWarning($"Duplicate Global ID found:");
                                Debug.LogWarning($"{captured.id}");
                                Debug.LogWarning($"{captured.name}");
                                Debug.LogWarning("------------------------------------");
                                continue;
                            }

                            processedGlobalIds.Add(captured.id);

                            int index = globalDictToSave.keys.IndexOf(captured.id);
                            if (index >= 0)
                            {
                                globalDictToSave.values[index] = captured.data;
                            }
                            else
                            {
                                globalDictToSave.keys.Add(captured.id);
                                globalDictToSave.values.Add(captured.data);
                            }
                        }
                        else
                        {
                            if (sceneDictToSave.keys.Contains(captured.id))
                            {
                                Debug.LogWarning($"Duplicate Scene ID found:");
                                Debug.LogWarning($"{captured.id}");
                                Debug.LogWarning($"{captured.name}");
                                Debug.LogWarning("------------------------------------");
                                continue;
                            }
                            
                            sceneDictToSave.keys.Add(captured.id);
                            sceneDictToSave.values.Add(captured.data);
                        }
                    }

                    SaveToFile(globalPath, globalDictToSave);
                    SaveToFile(scenePath, sceneDictToSave);
                });
            }
            finally
            {
                UIEvents.InvokeLoadingEnd();
            }
        }

        /*
        List<Persister> FindAllDataPersistenceObjects()
        {
            IEnumerable<Persister> dataPersistenceObjects = FindObjectsByType<Persister>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return new List<Persister>(dataPersistenceObjects);
        }
        */

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
    
        private Task RunTask(Action action)
        {
#if UNITY_WEBGL
            action();
            return Task.CompletedTask;
#else
            return Task.Run(action);
#endif
        }

        private Task<T> RunTask<T>(Func<T> func)
        {
#if UNITY_WEBGL
            return Task.FromResult(func());
#else
            return Task.Run(func);
#endif
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