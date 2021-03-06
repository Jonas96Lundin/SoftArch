using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Audio
/// Created by: Jonas
/// </summary>
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    /// <summary>
    /// Plays the sound that is chosen.
    /// Method by: Jonas
    /// </summary>
    /// <param name="sound">Input the name of the sound</param>
    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    /// <summary>
    /// Stops the sound that is chosen.
    /// Method by: Jonas
    /// </summary>
    /// <param name="sound">Input the name of the sound</param>
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    /// <summary>
    /// Sets the volume.
    /// Method by; Jonas
    /// </summary>
    /// <param name="volume">Insert the volume as a float</param>
    public void SetVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (!s.isSFX)
            {
                s.volume = volume;
                s.source.volume = volume;

            }
        }
    }
    /// <summary>
    /// Sets the volume.
    /// Method by; Jonas
    /// </summary>
    /// <param name="volume">Insert the volume as a float</param>
    public void SetVolumeSFX(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.isSFX)
            {
                s.volume = volume;
                s.source.volume = volume;

            }
        }
    }

    /// <summary>
    /// Checks if the sound is playing
    /// Method by: Jonas
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public bool IsSoundPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s.source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
