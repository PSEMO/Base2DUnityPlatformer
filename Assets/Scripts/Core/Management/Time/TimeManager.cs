using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Core.Management
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TimeScaleSO timeScaleData;

        private void OnEnable()
        {
            UIEvents.OnLoadingStart += PauseGameTime;
            UIEvents.OnLoadingEnd += UnpauseGameTime;

            UIEvents.OnGamePause += PauseGameTime;
            UIEvents.OnGameUnpause += UnpauseGameTime;
        }

        private void OnDisable()
        {
            UIEvents.OnLoadingStart -= PauseGameTime;
            UIEvents.OnLoadingEnd -= UnpauseGameTime;

            UIEvents.OnGamePause -= PauseGameTime;
            UIEvents.OnGameUnpause -= UnpauseGameTime;
        }

        private void PauseGameTime()
        {
            Time.timeScale = timeScaleData.pauseTimeScale;
        }

        private void UnpauseGameTime()
        {
            Time.timeScale = timeScaleData.playTimeScale;
        }
    }
}