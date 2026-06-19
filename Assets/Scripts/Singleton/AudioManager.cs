using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private List<AudioSource> audioSources;
    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();

        audioSources = new List<AudioSource>();
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return null;
    }

    private AudioSource CreateSource()
    {
        AudioSource createdSource = gameObject.AddComponent<AudioSource>();

        audioSources.Add(createdSource);

        return createdSource;
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        AudioSource currentSource = GetAvailableSource();

        if (currentSource == null)
        {
            currentSource = CreateSource();
        }

        currentSource.clip = clip;
        currentSource.volume = volume;
        currentSource.pitch = pitch;
        currentSource.loop = loop;
        currentSource.Play();
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopAllSounds()
    {
        foreach (AudioSource source in audioSources)
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
            return;
        }
        
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            return;
        }

        musicSource.volume = volume;
    }
}