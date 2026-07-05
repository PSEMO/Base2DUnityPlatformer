using UnityEngine;

namespace PSEMO.Core.Management
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "SO/Scene")]
    public class SceneSO : ScriptableObject
    {
        public int mainMenuSceneIndex = 0;
        public int firstGameSceneIndex = 1;
    }
}