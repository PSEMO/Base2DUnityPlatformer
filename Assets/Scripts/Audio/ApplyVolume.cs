using UnityEngine;
using UnityEngine.UI;

namespace PSEMO.Audio
{
    public class ApplyVolume : MonoBehaviour
    {
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;

        void Start()
        {
            masterSlider.value = AudioManager.Instance.MasterVolume;
            musicSlider.value = AudioManager.Instance.MusicVolume;
            sfxSlider.value = AudioManager.Instance.SFXVolume;
        }

        public void ApplyMaster()
        {
            AudioManager.Instance.MasterVolume = masterSlider.value;
        }

        public void ApplyMusic()
        {
            AudioManager.Instance.MusicVolume = musicSlider.value;
        }

        public void ApplySFX()
        {
            AudioManager.Instance.SFXVolume = sfxSlider.value;
        }
    }
}