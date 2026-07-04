using UnityEngine;

namespace PSEMO.UI
{
    [CreateAssetMenu(fileName = "UIData", menuName = "SO/UI")]
    public class UISO : ScriptableObject
    {
        public float returningFromPauseCooldown = 1.0f;
        public float extraDelayForLoading = 0.3f;

        [Space]

        public int firstGameSceneIndex = 1;
        public int mainMenuIndex = 0;
    }
}