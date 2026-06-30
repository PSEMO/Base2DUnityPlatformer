using System.Collections.Generic;
using UnityEngine;

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

        LoadVolumes();
        
        allAudios.Init();
        
        sourceToData = new Dictionary<AudioSource, AudioSO>();

        sfxSources = new List<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
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

    [SerializeField] private AllAudioSOs allAudios;

    private Dictionary<AudioSource, AudioSO> sourceToData;

    private List<AudioSource> sfxSources;
    private AudioSource musicSource;

    public void PlayAudio(string ID, bool isMusic = false)
    {
        AudioSO data = allAudios.GetAudioData(ID);
        PlayAudio(data, isMusic);
    }

    public void PlayAudio(AudioSO data, bool isMusic = false)
    {
        AudioSource source = isMusic? musicSource : GetAvailableSource();

        if (isMusic && source.clip == data.clip && source.isPlaying)
            return;

        if (sourceToData.ContainsKey(source))
            sourceToData[source] = data;
        else
            sourceToData.Add(source, data);

        source.clip = data.clip;
        source.loop = data.loop;
        source.volume = data.volume * sfxVolume * masterVolume;

        source.Play();
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
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
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

        musicSource.volume = raw * musicVolume * masterVolume;
    }

    private void ApplySFXVolumes()
    {
        if (sfxSources == null) return;

        foreach (AudioSource source in sfxSources)
        {
            float raw = sourceToData.ContainsKey(source)
                ? sourceToData[source].volume
                : 1f;

            source.volume = raw * sfxVolume * masterVolume;
        }
    }
}