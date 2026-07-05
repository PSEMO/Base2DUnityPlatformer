using UnityEngine.SceneManagement;
using PSEMO.Events;

namespace PSEMO.Core.Management
{
    public static class SceneLoader
    {
        public static void Load(string sceneName)
        {
            UIEvents.InvokeLoadingStart();
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.completed += _ => UIEvents.InvokeLoadingEnd();
        }

        public static void Load(int sceneIndex)
        {
            UIEvents.InvokeLoadingStart();
            var op = SceneManager.LoadSceneAsync(sceneIndex);
            op.completed += _ => UIEvents.InvokeLoadingEnd();
        }
    }
}