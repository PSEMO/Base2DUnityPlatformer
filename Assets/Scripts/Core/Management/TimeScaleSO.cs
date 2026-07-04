using UnityEngine;

namespace PSEMO.Core.Management
{
    [CreateAssetMenu(fileName = "TimeScaleData", menuName = "SO/TimeScale")]
    public class TimeScaleSO : ScriptableObject
    {
        public float pauseTimeScale = 0f;
        public float playTimeScale = 1f;
    }
}
