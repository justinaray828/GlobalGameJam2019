using UnityEngine;
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
    private DistortionEffects de;

    SpeechBubbleSettings settings = new SpeechBubbleSettings();


    private void Start()
    {
        MainGame.SetActive(true);
        DancingGame.SetActive(false);
        FridgeGame.SetActive(false);
        FlowerGame.SetActive(false);

        settings.MaxWidth = 300;
        settings.TimeBetweenChars = 0.03f;
        settings.TimeUntilClose = 3;

        de = FindObjectOfType<DistortionEffects>();
    }

    private bool _SavedPass;
    private string _SavedPuzzleName;


    //TODO: set distortion level 
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



        if (pass)
        {
            if (puzzlename == "dance")
            {
                if (danceSolved == false)
                {
                    de.SetDistortionLevel(de.GetDistortionLevel() + 1);
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                    settings.Text = "That was our wedding dance. It took me forever to learn it.";
                    SpeechBubbleSettings[] settingsArray = { settings };
                    SpeechBubble.Instance.DisplaySpeech(settingsArray);
                }
                danceSolved = true;
            }
            else if (puzzlename == "flower")
            {
                if (flowerSolved == false)
                {
                    de.SetDistortionLevel(de.GetDistortionLevel() + 1);
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                }
                flowerSolved = true;
            }
            else if (puzzlename == "fridge")
            {
                if (fridgeSolved == false)
                {
                    de.SetDistortionLevel(de.GetDistortionLevel() + 1);
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
            string[] texts = new string[] { "That doesn't seem quite right...", "I can't seem to remember that correctly...", "There's something there... but that's not it..." };
            settings.Text = texts[UnityEngine.Random.Range(1, 3)];
            SpeechBubbleSettings[] settingsArray = { settings };
            SpeechBubble.Instance.DisplaySpeech(settingsArray);

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

    public bool CheckIfFlowerAllowed() //could be done with out parameter
    {
        if (danceSolved && fridgeSolved)
        {
            return true;
        }
        else
        {
            if (!danceSolved && !fridgeSolved)
                settings.Text = "I want to return to the fridge and radio before I mess with that";
            else if (!danceSolved)
            {
                settings.Text = "I want to return to the radio before I mess with this";
            }
            else
            {
                settings.Text = "I want to return to the fridge before I mess with this";
            }
            SpeechBubbleSettings[] settingsArray = { settings };
            SpeechBubble.Instance.DisplaySpeech(settingsArray);

            return false;
        }
    }
}