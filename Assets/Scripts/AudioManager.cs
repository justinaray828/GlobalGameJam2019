using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    private int currentScoreNum;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
       
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //s.source.outputAudioMixerGroup

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        Debug.Log("in start");
        Play("score1");
        currentScoreNum = 1;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Playing sound:'" + name + "' but was not found");
            return;
        }
        Debug.Log("playing: " + name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Stopping sound:'" + name + "' but was not found");
            return;
        }
        Debug.Log("stopping: " + name);
        s.source.Stop();
    }

    public void IncreaseScore()
    {
        Stop("score" + currentScoreNum);
        currentScoreNum++;
        Play("score" + currentScoreNum);
    }
}
