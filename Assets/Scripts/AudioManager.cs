using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(.05f, 2f)]
    public float fadespeed = 1f;
    public AudioMixer masterMixer;
    public AudioMixerSnapshot scoreOnSnapshot;
    public AudioMixerSnapshot scoreOffSnapshot;
    public AudioMixerSnapshot scorefinalSnap;
    [Range(.5f,5f)]
    public float transitionSpeed = 1f;

    public Sound[] sounds;
    public static AudioManager instance;

    private int currentScoreNum;
    private bool fadeInState;
    private bool fadeOutState;
    private Sound fadeInSound;
    private Sound fadeOutSound;
    private string walking;

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

            s.source.outputAudioMixerGroup = s.output;

            //sources were not truly playing on awake. Perhaps they were being created after awake had been called?
            //regardless, this manually takes 
            if (s.playonawake)
                s.source.Play();
        }
    }

    void Start()
    {   
        //start all score tracks simultaneously
        currentScoreNum = 1;
        for (int i = 4; i > 0; i--)
        {
            Play("score" + i);
        }

        //set all but first score track to 0 volume
        currentScoreNum = 1;
        for (int i = 4; i > 1; i--)
        {
            Sound s = Array.Find(sounds, sound => sound.name == "score"+i);
            s.source.volume = 0f;
        }
        //turn score volume on, puzzle volume off
        TransitionSnapshot(scoreOnSnapshot);
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
        s.source.volume = 1f;
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

    public void TransitionSnapshot(AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(transitionSpeed);
    }

    /// <summary>
    /// To be called when new memory has been remembered. This
    /// will activate the next layer of the musical score
    /// </summary>
    private void IncreaseScore()
    {
        currentScoreNum++;
        Play("win1");
        Debug.Log("increasing score");
        Sound s = Array.Find(sounds, sound => sound.name == "score"+currentScoreNum);
        s.source.volume = 1f;
    }

    public void PlayWalk()
    {
        if (walking == null)
        {
            string newwalk = "walk" + UnityEngine.Random.Range(1, 5);
            Play(newwalk);
            walking = newwalk;
        }
    }
    public void StopWalk()
    {
        if (walking != null) { 
            Stop(walking);
            walking = null;
        }
    }

    public void ToHomeMusicOnSuccess()
    {
        IncreaseScore();
        if (currentScoreNum > 3)
        {
            Play("scorefinal");
            TransitionSnapshot(scorefinalSnap);
        }
        else { 
            TransitionSnapshot(scoreOnSnapshot);
        }
        Stop("puzzleFridge");
        Stop("puzzleFlower");
        Stop("puzzleDance");
    }

    public void ToHomeMusicOnFailure()
    {
        TransitionSnapshot(scoreOnSnapshot);
        Stop("puzzleFridge");
        Stop("puzzleFlower");
        Stop("puzzleDance");
    }

    public void ToDanceMusic()
    {
        Play("puzzleDance");
        TransitionSnapshot(scoreOffSnapshot);
    }

    public void ToFridgeMusic()
    {
        Play("puzzleFridge");
        TransitionSnapshot(scoreOffSnapshot);
    }

    public void ToFlowerMusic()
    {
        Play("puzzleFlower");
        TransitionSnapshot(scoreOffSnapshot);
    }

}
