using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractableObjectData interactableObjectData;

    private GameChangeInformation gameChangeInformation;
    private const string GAMECHANGEINFORMATIONTAG = "GameChangeInformation";

    private GameObject imageClueCanvas;
    private const string IMAGECLUETAG = "ImageClue";

    public Transform ParticleSystemAttachmentPoint;
    public GameObject ParticleSystemPrefab;
    public AudioManager am;
    private ParticleSystem ParticleSys;
    private MeshCollider Collider;

    private Outline _Outline;

    private void Start()
    {
        gameChangeInformation = GameObject.FindGameObjectWithTag(GAMECHANGEINFORMATIONTAG).GetComponent<GameChangeInformation>();
        imageClueCanvas = GameObject.FindGameObjectWithTag(IMAGECLUETAG);
        GameObject particleSystem = GameObject.Instantiate(ParticleSystemPrefab);
        ParticleSys = particleSystem.GetComponent<ParticleSystem>();
        particleSystem.transform.parent = ParticleSystemAttachmentPoint;
        particleSystem.transform.localPosition = Vector3.zero;
        particleSystem.transform.localRotation = Quaternion.identity;
        am = FindObjectOfType<AudioManager>();

        Collider = GetComponent<MeshCollider>();

        _Outline = gameObject.AddComponent<Outline>();
        _Outline.enabled = false;
        _Outline.color = 1;
    }

    private void Update()
    {
        if (PlayerIsNear())
        {
            if (ParticleSys.isPlaying == false)
            {
                ParticleSys.Play();
            }
        }
        else
        {
            if (ParticleSys.isPlaying == true)
            {
                ParticleSys.Stop();
            }
        }
    }

    public bool SetHover(bool inHover)
    {
        if (PlayerIsNear() == false)
        {
            _Outline.enabled = false;
            return false;
        }
        
        if (_Outline.enabled == false &&
            inHover == true)
        {
            _Outline.enabled = true;
        }
        else if (_Outline.enabled == true &&
                 inHover == false)
        {
            _Outline.enabled = false;
        }

        return true;
    }

    /// <summary>
    /// Interact with clicked GameObject
    /// </summary>
    public void Interact()
    {
        if (PlayerIsNear() == false)
        {
            return;
        }

        switch(interactableObjectData.interactableObjectType)
        {
            case InteractableObjectData.InteractableObjectType.Text:
                am.Play("bubble1");
                TextInteraction();
                break;
            case InteractableObjectData.InteractableObjectType.Image:
                Debug.Log(interactableObjectData.name);
                if (interactableObjectData.name == "BookShelf")
                    am.Play("paper1");
                ImageInteraction();
                break;
            case InteractableObjectData.InteractableObjectType.MiniGame:
                MiniGameInteraction();
                break;
        }
    }

    private bool PlayerIsNear()
    {
        Vector3 playerPosition = PlayerMovement.Instance.transform.position;
        return Vector3.Distance(playerPosition, Collider.bounds.center) <= 10f;
    }

    private void TextInteraction()
    {
        SpeechBubble.Instance.DisplaySpeech(interactableObjectData.outputText);
    }

    private void ImageInteraction()
    {
        GameObject image = GameObject.Instantiate(interactableObjectData.clue, imageClueCanvas.transform);
        CloseImageInput imageClose = image.GetComponentInChildren<CloseImageInput>();
        imageClose.RegisterOnClose(ImageCloseSpeech);
    }

    private void ImageCloseSpeech(CloseImageInput imageClose)
    {
        SpeechBubbleSettings settings1 = new SpeechBubbleSettings();
        settings1.MaxWidth = 300;
        settings1.TimeUntilClose = 3;
        settings1.TimeBetweenChars = 0.03f;

        SpeechBubbleSettings settings2 = new SpeechBubbleSettings();
        settings2.MaxWidth = 300;
        settings2.TimeUntilClose = 3;
        settings2.TimeBetweenChars = 0.03f;

        switch(interactableObjectData.clueType)
        {
            case PictureType.Baby:
                settings1.Text = "Baby pic text";
                settings2.Text = "Text 2";
                break;
            case PictureType.Book:
                settings1.Text = "That taco recipe sounds delicious.";
                settings2.Text = "It's always important to add love.";
                break;
            case PictureType.Dancing:
                settings1.Text = "If only you knew how to dance.";
                settings2.Text = "You could take that girl next door waltzing.";
                break;
            case PictureType.Honeymoon:
                settings1.Text = "Maybe you’re going to the beach with mom and dad today?";
                settings1.MaxWidth = 400;
                settings2.Text = "….Where are they, anyway?";
                break;
            case PictureType.Kid:
                settings1.Text = "You haven’t spoken to your grandpa in…it must be *months* now.";
                settings2.Text = "Is that it? Did you forget to call him for his birthday?";
                break;
            case PictureType.Mountain:
                settings1.Text = "The painting says “Icy wintertime”";
                settings2.Text = "It calls to mind lovely, blooming flowers, and a woman calling out in the storm.";
                settings2.MaxWidth = 500;
                break;
            case PictureType.Snow:
                settings1.Text = "You remember going skiing with your family.";
                settings2.Text = "Who was the lady that was with you though?";
                break;
            default: 
                break;
        }
        SpeechBubbleSettings[] settingsArray = {settings1, settings2};
        SpeechBubble.Instance.DisplaySpeech(settingsArray);
        imageClose.UnregisterOnClose(ImageCloseSpeech);
    }

    private void MiniGameInteraction()
    {
        if(interactableObjectData.miniGameType.ToString() == "Flowers")
        {
            if (!gameChangeInformation.CheckIfFlowerAllowed())
            {
                return;
            }
        }
        FadedToAndFromBlackManager.Instance.RegisterForFinishedFading(ChangeToMiniGameOnFadeFinished);
        FadedToAndFromBlackManager.Instance.FadeToBlack();
    }

    private void ChangeToMiniGameOnFadeFinished()
    {
        FadedToAndFromBlackManager.Instance.UnregisterForFinishedFading(ChangeToMiniGameOnFadeFinished);
        
        Debug.Log("Game Change to: " + interactableObjectData.miniGameType.ToString());

        switch (interactableObjectData.miniGameType)
        {
            case InteractableObjectData.MiniGameType.Dancing:
                gameChangeInformation.ChangeToDancingGame();
                break;
            case InteractableObjectData.MiniGameType.Fridge:
                gameChangeInformation.ChangeToFridgeGame();
                break;
            case InteractableObjectData.MiniGameType.Flowers:
                if(gameChangeInformation.CheckIfFlowerAllowed())
                    gameChangeInformation.ChangeToFlowerGame();
                break;
            default:
                Debug.LogError("Mini game was activated but no mini game was chosen");
                break;
        }
    }
}
