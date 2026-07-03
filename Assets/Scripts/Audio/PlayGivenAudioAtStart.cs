using UnityEngine;

namespace PSEMO.Audio
{
    public class PlayGivenAudioAtStart : MonoBehaviour
    {
        [SerializeField] string AudioName;

        void Start()
        {
            AudioManager.Instance.PlayAudio(AudioName, true);
        }
    }
}