using UnityEngine.Audio;
using UnityEngine;
using System;
using Assets.Generics;

[Serializable]
public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;

    public void Init()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " not found!");
            return;
        }
        s.source.Play();
    }

}