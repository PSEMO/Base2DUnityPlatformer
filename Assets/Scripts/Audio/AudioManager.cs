using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSEMO.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        
            allAudios.Init();
        
            sourceToData = new Dictionary<AudioSource, AudioSO>();

            sfxSources = new List<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            secondaryMusicSource = gameObject.AddComponent<AudioSource>();

            LoadVolumes();
        }

        private const string MASTER_VOL_KEY = "AudioMaster";
        private const string MUSIC_VOL_KEY = "AudioMusic";
        private const string SFX_VOL_KEY = "AudioSFX";

        private static readonly float defaultVolume = 0.75f;

        private float masterVolume = defaultVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(MASTER_VOL_KEY, masterVolume);
                ApplyMusicVolume();
                ApplySFXVolumes();
            }
        }

        private float musicVolume = defaultVolume;
        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(MUSIC_VOL_KEY, musicVolume);
                ApplyMusicVolume();
            }
        }

        private float sfxVolume = defaultVolume;
        public float SFXVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(SFX_VOL_KEY, sfxVolume);
                ApplySFXVolumes();
            }
        }

        void Start()
        {
            ApplyMusicVolume();
            ApplySFXVolumes();
        }

        [SerializeField] private AllAudioSOs allAudios;

        [SerializeField] private float musicTransitionDuration = 1.0f;
        private Coroutine musicTransitionCoroutine;

        private Dictionary<AudioSource, AudioSO> sourceToData;

        private List<AudioSource> sfxSources;
        private AudioSource musicSource;
        private AudioSource secondaryMusicSource;

        public void PlayAudio(string ID, bool isMusic = false)
        {
            AudioSO data = allAudios.GetAudioData(ID);
            PlayAudio(data, isMusic);
        }

        public void PlayAudio(AudioSO data, bool isMusic = false)
        {
            if (isMusic)
            {
                if (musicSource.clip == data.clip && musicSource.isPlaying)
                    return;

                if (musicSource.isPlaying)
                {
                    if (musicTransitionCoroutine != null)
                    {
                        StopCoroutine(musicTransitionCoroutine);
                        ResetCrossfadeMusic(data);
                    }

                    musicTransitionCoroutine = StartCoroutine(CrossfadeMusic(data));
                    return;
                }

                PlayAudio(musicSource, data, true);
                return;
            }

            AudioSource source = GetAvailableSource();

            PlayAudio(source, data);
        }

        public void PlayAudio(AudioSource source, AudioSO data, bool isMusic = false, float volume = -1)
        {
            sourceToData[source] = data;

            source.clip = data.clip;
            source.loop = data.loop;
            source.volume = volume == -1? GetVolumeOfData(data, isMusic) : volume;

            source.Play();
        }

        private IEnumerator CrossfadeMusic(AudioSO newData)
        {
            (musicSource, secondaryMusicSource) = (secondaryMusicSource, musicSource);
            
            float startSecondaryVolume = secondaryMusicSource.volume;

            PlayAudio(musicSource, newData, true, 0f);

            float t = 0;
            while (t < musicTransitionDuration)
            {
                t += Time.deltaTime;
                float normalizedTime = t / musicTransitionDuration;
                
                float currentTargetVolume = GetVolumeOfData(newData, true);
                
                musicSource.volume = Mathf.Lerp(0f, currentTargetVolume, normalizedTime);
                secondaryMusicSource.volume = Mathf.Lerp(startSecondaryVolume, 0f, normalizedTime);
                
                yield return null;
            }
            
            ResetCrossfadeMusic(newData);
        }

        private float LinearToLog(float value)
        {
            return Mathf.Clamp01(value * value);
        }

        private float GetVolumeOfData(AudioSO data, bool isMusic = false) =>
            LinearToLog(isMusic? musicVolume : sfxVolume) * data.volume * LinearToLog(masterVolume);

        private void ResetCrossfadeMusic(AudioSO currentData)
        {
            musicSource.volume = GetVolumeOfData(currentData, true);
            secondaryMusicSource.Stop();
            secondaryMusicSource.volume = 0f;

            musicTransitionCoroutine = null;
        }

        public void StopAllSounds()
        {
            foreach (AudioSource source in sfxSources)
            {
                if (source.isPlaying)
                {
                    source.Stop();
                }
            }
        }

        public void StopMusic()
        {
            if (musicTransitionCoroutine != null)
            {
                StopCoroutine(musicTransitionCoroutine);
                musicTransitionCoroutine = null;
            }

            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
            
            if (secondaryMusicSource != null && secondaryMusicSource.isPlaying)
            {
                secondaryMusicSource.Stop();
            }
        }

        private void LoadVolumes()
        {
            masterVolume = PlayerPrefs.GetFloat(MASTER_VOL_KEY, defaultVolume);
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, defaultVolume);
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY, defaultVolume);
        }

        public void SaveVolumes()
        {
            PlayerPrefs.Save();
        }

        private AudioSource GetAvailableSource()
        {
            foreach (AudioSource source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            return CreateSource();
        }

        private AudioSource CreateSource()
        {
            AudioSource createdSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(createdSource);
            return createdSource;
        }

        private void ApplyMusicVolume()
        {
            if (musicSource == null) return;

            float raw = sourceToData.ContainsKey(musicSource)
                ? sourceToData[musicSource].volume
                : 1f;

            if (musicTransitionCoroutine == null)
            {
                musicSource.volume = raw * LinearToLog(musicVolume) * LinearToLog(masterVolume);
            }
        }

        private void ApplySFXVolumes()
        {
            if (sfxSources == null) return;

            foreach (AudioSource source in sfxSources)
            {
                float raw = sourceToData.ContainsKey(source)
                    ? sourceToData[source].volume
                    : 1f;

                source.volume = raw * LinearToLog(sfxVolume) * LinearToLog(masterVolume);
            }
        }
    }
}