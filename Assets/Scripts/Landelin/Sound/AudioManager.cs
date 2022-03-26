using UnityEngine.Audio;
using UnityEngine;
using System;

/// <summary>
/// Class based on Brackeys tutorial : https://www.youtube.com/watch?v=6OT43pvUyfY 
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// List of sounds we want to play
    /// </summary>
    public Sound[] sounds;

    /// <summary>
    /// Instance of the AudioManager.
    /// </summary>
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start()
    {
        //Play("mainTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public bool checkIsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying; 
    }
}
