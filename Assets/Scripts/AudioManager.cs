﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(.05f, 2f)]
    public float fadespeed = 1f;
    public Sound[] sounds;
    public static AudioManager instance;

    private int currentScoreNum;
    private bool fadeInState;
    private bool fadeOutState;
    private Sound fadeInSound;
    private Sound fadeOutSound;

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
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playonawake;

            //sources were not truly playing on awake. Perhaps they were being created after awake had been called?
            //regardless, this manually takes 
            if (s.playonawake)
                s.source.Play();
        }
    }

    void Start()
    {   
        currentScoreNum = 1;
    }

    void Update()
    {
        //TODO consider changing this to having a stack of things fade.
        if (fadeInState)
        {
            fadeInSound.source.volume += fadespeed * Time.deltaTime;
            if (fadeInSound.source.volume >= 1f)
                fadeInState = false;
        }
        if (fadeOutState) {
            fadeOutSound.source.volume -= fadespeed * Time.deltaTime;
            if (fadeOutSound.source.volume <= 0f)
                fadeOutState = false;
        }
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

    /// <summary>
    /// Fade in the given sound
    /// </summary>
    /// <param name="name">Name.</param>
    public void FadeIn(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Fading sound:'" + name + "' but was not found");
            return;
        }
        Debug.Log("fading in: " + name);
        fadeInSound = s;
        fadeInState = true;
    }

    /// <summary>
    /// Fades the out.
    /// </summary>
    /// <param name="name">Name.</param>
    public void FadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Fading sound:'" + name + "' but was not found");
            return;
        }
        Debug.Log("fading out: " + name);
        fadeOutSound = s;
        fadeOutState = true;
    }

    /// <summary>
    /// To be called when new memory has been remembered. This
    /// will activate the next layer of the musical score
    /// </summary>
    private void IncreaseScore()
    {
        currentScoreNum++;
    }

    public void ToHomeMusicOnSuccess()
    {
        IncreaseScore();
        Play("score" + currentScoreNum);
    }

    public void ToHomeMusicOnFailure()
    {
        Play("score" + currentScoreNum);
    }

    public void ToDanceMusic()
    {
        FadeOut("score" + currentScoreNum);
        Play("puzzleDance");
        FadeIn("puzzleDance");
    }

    public void ToFridgeMusic()
    {
        FadeOut("score" + currentScoreNum);
        Play("puzzleFridge");
        FadeIn("puzzleFridge");
    }

    public void ToFlowerMusic()
    {
        FadeOut("score" + currentScoreNum);
        Play("puzzleFlower");
        FadeIn("puzzleFlower");
    }
}
