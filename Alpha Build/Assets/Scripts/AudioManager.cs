using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;

    public AudioSource themeSource;
    [Range(0f, 1f)]
    public float startingVolume = 0.7f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        themeSource = GetComponent<AudioSource>();

        foreach (Sound s in sounds)
        {
            switch (s.type)
            {
                case SoundType.Theme:
                    s.source = themeSource;
                    s.source.loop = s.loop;
                    s.source.outputAudioMixerGroup = mixerGroup;
                    break;
                case SoundType.GlobalSound:
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;
                    s.source.loop = s.loop;
                    s.source.outputAudioMixerGroup = mixerGroup;
                    break;
                case SoundType.LocalSound:
                    break;
                default:
                    Debug.Log("sound type not recognized");
                    break;
            }
        }
    }


    private void Start()
    {
        Play("MainMenuTheme");
        setVolume(startingVolume);
    }

    public void Play(string sound, AudioSource source = null)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
        if (s.clip == null)
        {
            Debug.LogWarning("Clip: " + s.name + " not found!");
            return;
        }
        switch (s.type)
        {
            case SoundType.Theme:
                s.source.clip = s.clip;
                s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
                s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
                s.source.Play();
                break;
            case SoundType.GlobalSound:
                s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
                s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
                s.source.Play();
                break;
            case SoundType.LocalSound:
                source.clip = s.clip;
                source.loop = s.loop;
                source.outputAudioMixerGroup = mixerGroup;
                source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
                source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
                source.spatialBlend = 1.0f; // Fully 3D sound
                //source.minDistance = 1.0f; // Minimum distance to hear the sound
                //source.maxDistance = 10.0f; // Maximum distance at which the sound can be heard
                source.Play();
                break;
        }
    }
    public void PlayLocal(string sound, AudioSource source)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
        if (s.clip == null)
        {
            Debug.LogWarning("Clip: " + s.name + " not found!");
            return;
        }
        source.clip = s.clip;
        source.loop = s.loop;
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        source.spatialBlend = 1.0f; // Fully 3D sound
        source.minDistance = 5.0f; // Minimal distance to hear the sound on full volume
        source.maxDistance = 100.0f; // Maximum distance at which the sound can be heard
        source.Play();
    }

    public void setVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ThemeTransition(string theme, float transitionTime)
    {
        Debug.Log("Started Theme Transition");
        StartCoroutine(TransitionCoroutine(theme, transitionTime));
    }

    private IEnumerator TransitionCoroutine(string theme, float transitionTime)
    {
        Sound s = Array.Find(sounds, item => item.name == theme);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            yield break;
        }

        // Fade out the current music
        float startVolume = themeSource.volume;
        float timer = 0f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            themeSource.volume = Mathf.Lerp(startVolume, 0f, timer / (transitionTime / 2));
            yield return null;
        }

        // Switch the music clip
        themeSource.Stop();
        themeSource.time = 0f;
        themeSource.clip = s.clip;

        // Fade in the new music
        themeSource.Play();
        timer = 0f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            themeSource.volume = Mathf.Lerp(0f, startVolume, timer / (transitionTime / 2));
            yield return null;
        }

        // Ensure the volume is back to normal
        themeSource.volume = startVolume;
    }

}