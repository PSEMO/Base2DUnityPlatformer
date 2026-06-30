using UnityEngine;

namespace PSEMO.Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "SO/Audio")]
    public class AudioSO : ScriptableObject
    {
        [Header("Can not repeat in any other file")]
        public string ID;
    
        [Header("Audio File")]
        public AudioClip clip;

        [Header("Settings")]
        [Range(0, 1)] public float volume = 1.0f;
        public bool loop = false;
    }
}