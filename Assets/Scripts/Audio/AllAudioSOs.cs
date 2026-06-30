using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.Audio
{
    [CreateAssetMenu(fileName = "AllAudiosData", menuName = "SO/AllAudios")]
    public class AllAudioSOs : ScriptableObject
    {
        [SerializeField] private List<AudioSO> _allAudios;
        private Dictionary<string, AudioSO> allAudiosID;

        public void Init()
        {
            allAudiosID = new Dictionary<string, AudioSO>();

            foreach(AudioSO audioSO in _allAudios)
            {
                allAudiosID.Add(audioSO.ID, audioSO);
            }
        }

        public AudioSO GetAudioData(string ID)
        {
            return allAudiosID[ID];
        }
    }
}