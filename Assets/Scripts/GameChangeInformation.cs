﻿using UnityEngine;
using System.Collections;

public class GameChangeInformation : MonoBehaviour
{
    [Header("Main House")]
    public GameObject MainGame;

    [Header("Dancing Game")]
    public GameObject DancingGame;

    [Header("Fridge Game")]
    public GameObject FridgeGame;

    [Header("Flower Game")]
    public GameObject FlowerGame;

    public SpeechBubble speechbub;

    private bool danceSolved = false;
    private bool fridgeSolved = false;
    private bool flowerSolved = false;

    private void Start()
    {
        MainGame.SetActive(true);
        DancingGame.SetActive(false);
        FridgeGame.SetActive(false);
        FlowerGame.SetActive(false);
    }

    private bool _SavedPass;
    private string _SavedPuzzleName;

    /// <summary>
    /// Changes focus back to main game. Pass in boolean
    /// on whether puzzle was completed.
    /// pass in "dance","flower",or "fridge" for string name
    /// TODO: have a better system than passing in a string
    /// </summary>
    /// <param name="pass">If set to <c>true</c> pass.</param>
    /// <param name="puzzlename">Puzzlename.</param>
    public void ChangeToMainGame(bool pass, string puzzlename = "none")
    {
        _SavedPass = pass;
        _SavedPuzzleName = puzzlename;
        
        FadedToAndFromBlackManager.Instance.RegisterForFinishedFading(ChangeToMainGameAfterFade);
        FadedToAndFromBlackManager.Instance.FadeToBlack();
    }

    private void ChangeToMainGameAfterFade()
    {
        FadedToAndFromBlackManager.Instance.UnregisterForFinishedFading(ChangeToMainGameAfterFade);
        
        bool pass = _SavedPass;
        string puzzlename = _SavedPuzzleName;
        
        if(puzzlename == "none")
            Debug.LogWarning("ChangeToMainGame called without 2nd parameter");
        FlowerGame.SetActive(false);
        FridgeGame.SetActive(false);
        DancingGame.SetActive(false);
        MainGame.SetActive(true);

        SpeechBubbleSettings settings = new SpeechBubbleSettings();
        settings.MaxWidth = 300;
        settings.TimeBetweenChars = 0.03f;
        settings.TimeUntilClose = 3;

        if (pass)
        {
            if (puzzlename == "dance") 
            {
                if (danceSolved == false)
                {
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                    settings.Text = "That was our wedding dance. It took me forever to learn it.";
                    SpeechBubbleSettings[] settingsArray = {settings};
                    SpeechBubble.Instance.DisplaySpeech(settingsArray);
                }
                danceSolved = true;
            }
            else if (puzzlename == "flower")
            {
                if (flowerSolved == false)
                {
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                }
                flowerSolved = true;
            }
            else if (puzzlename == "fridge")
            {
                if (fridgeSolved == false)
                {
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                }
                fridgeSolved = true;
            }
            //if(danceSolved && flowerSolved && fridgeSolved)
            //TODO:insert final game ending events
            Debug.Log("reached success");
            //TODO: insert flavor text for both success and failure.
            FindObjectOfType<AudioManager>().ToHomeMusicOnFailure();
        }
        else
        {
            FindObjectOfType<AudioManager>().ToHomeMusicOnFailure();
            Debug.Log("Reached Fail State");
        }
        
        FadedToAndFromBlackManager.Instance.FadeFromBlack();
    }

    public void ChangeToDancingGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToDanceMusic();
        DancingGame.SetActive(true);
        
        FadedToAndFromBlackManager.Instance.FadeFromBlack();
    }

    public void ChangeToFridgeGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToFridgeMusic();
        FridgeGame.SetActive(true);
        
        FadedToAndFromBlackManager.Instance.FadeFromBlack();
    }

    public void ChangeToFlowerGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToFlowerMusic();
        FlowerGame.SetActive(true);
        
        FadedToAndFromBlackManager.Instance.FadeFromBlack();
    }
}