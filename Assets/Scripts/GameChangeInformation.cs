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
    public GameObject EndGame;
    public GameObject DiningRoomLight;

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

        de = FindObjectOfType<DistortionEffects>();

        settings.MaxWidth = 300;
        settings.TimeBetweenChars = 0.03f;
        settings.TimeUntilClose = 3;
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
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                    settings.Text = "There was a girl next door who loved to dance.";
                    settings.MaxWidth = 300;

                    SpeechBubbleSettings speech2 = new SpeechBubbleSettings();
                    SpeechBubbleSettings speech3 = new SpeechBubbleSettings();

                    speech2.Text = "She had three left feet, but she sure did have fun.";
                    speech3.Text = "Maybe you’ll see her again someday…";

                    speech2.MaxWidth = 300;
                    speech3.MaxWidth = 200;

                    speech2.TimeBetweenChars = speech3.TimeBetweenChars = 0.03f;
                    speech2.TimeUntilClose = speech3.TimeUntilClose = 2;

                    SpeechBubbleSettings[] settingsArray = { settings, speech2, speech3 };
                    SpeechBubble.Instance.DisplaySpeech(settingsArray);
                }
                de.SetDistortionLevel(de.GetDistortionLevel() + 1);

                danceSolved = true;
            }
            else if (puzzlename == "flower")
            {
                if (flowerSolved == false)
                {
                    settings.Text = "That’s it! It’s the remembering-lady’s birthday today.";
                    settings.MaxWidth = 300;

                    SpeechBubbleSettings speech2 = new SpeechBubbleSettings();

                    speech2.Text = "She liked purple flowers too, just like the dancing girl…";

                    speech2.MaxWidth = 300;

                    speech2.TimeBetweenChars = 0.03f;
                    speech2.TimeUntilClose = 2;

                    SpeechBubbleSettings[] settingsArray = { settings, speech2 };
                    SpeechBubble.Instance.DisplaySpeech(settingsArray);
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                }
                de.SetDistortionLevel(de.GetDistortionLevel() + 1);

                flowerSolved = true;
            }
            else if (puzzlename == "fridge")
            {
                if (fridgeSolved == false)
                {
                    settings.Text = "The dancing girl…";
                    settings.MaxWidth = 200;

                    SpeechBubbleSettings speech2 = new SpeechBubbleSettings();
                    SpeechBubbleSettings speech3 = new SpeechBubbleSettings();

                    speech2.Text = "You wrote a poem for her just like this once.";
                    speech3.Text = "She always did love purple flowers.";

                    speech2.MaxWidth = 300;
                    speech3.MaxWidth = 200;

                    speech2.TimeBetweenChars = speech3.TimeBetweenChars = 0.03f;
                    speech2.TimeUntilClose = speech3.TimeUntilClose = 2;

                    SpeechBubbleSettings[] settingsArray = { settings, speech2, speech3 };
                    SpeechBubble.Instance.DisplaySpeech(settingsArray);
                    FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
                }
                de.SetDistortionLevel(de.GetDistortionLevel() + 1);

                fridgeSolved = true;
            }
            //if(danceSolved && flowerSolved && fridgeSolved)
            //TODO:insert final game ending events
            //TODO: insert flavor text for both success and failure.
            FindObjectOfType<AudioManager>().ToHomeMusicOnFailure();
        }
        else
        {
            string[] texts = new string[] { "The memory slips through your fingers and into air.", "No, that’s not it. The memory fails you.", "The moment passes. The memory fades." };
            settings.Text = texts[UnityEngine.Random.Range(1, 3)];
            SpeechBubbleSettings[] settingsArray = { settings };
            SpeechBubble.Instance.DisplaySpeech(settingsArray);

            FindObjectOfType<AudioManager>().ToHomeMusicOnFailure();
        }
        
        FadedToAndFromBlackManager.Instance.FadeFromBlack();

        if (danceSolved == true &&
            fridgeSolved == true &&
            flowerSolved == true)
        {
            EndGame.SetActive(true);
            DiningRoomLight.SetActive(false);
        }
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
                settings.Text = "You want to return to the fridge and radio before you mess with that";
            else if (!danceSolved)
            {
                settings.Text = "You want to return to the radio before you mess with this";
            }
            else
            {
                settings.Text = "You want to return to the fridge before you mess with this";
            }
            SpeechBubbleSettings[] settingsArray = { settings };
            SpeechBubble.Instance.DisplaySpeech(settingsArray);

            return false;
        }
    }
}