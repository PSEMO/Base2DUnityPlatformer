using UnityEngine;

namespace PSEMO.Audio
{
    public class PlayGivenAudioAtAwake : MonoBehaviour
    {
        [SerializeField] string AudioName;

        void Start()
        {
            AudioManager.Instance.PlayAudio(AudioName, true);
        }
    }
}