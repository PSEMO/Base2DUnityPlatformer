using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSEMO.Diagnostic
{
    #if UNITY_EDITOR
    
    /*
    OnSceneLoaded measures from:
        Very begining of the last scenes death (before OnDestroy and stuff)
                    TO
        Just before the next scenes Start (After Awake and OnEnabled)
        
    Update measures:
        Everything of a frame. (including; Awake/OnEnabled/Start for the first frame)
    */

    public class SloppyLagDetector : MonoBehaviour
    {
        public static SloppyLagDetector Instance { get; private set; }

        private float loadStartTime;
        private bool isLoading;

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
                
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneUnloaded(Scene current)
        {
            loadStartTime = Time.realtimeSinceStartup;
            isLoading = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (isLoading)
            {
                float timeTaken = (Time.realtimeSinceStartup - loadStartTime) * 1000f;
                Debug.LogWarning($"[SloppyLagDetector] Scene '{scene.name}' loaded in {timeTaken:F2} ms!");
                isLoading = false;
            }
        }

        void Update()
        {
            if (Time.deltaTime > 0.05f)
            {
                Debug.LogWarning($"[SloppyLagDetector] LAG SPIKE DETECTED: Frame took {Time.deltaTime * 1000f:F2} ms!");
            }
        }
    }
    #endif
}