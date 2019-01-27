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
        Instantiate(interactableObjectData.clue, imageClueCanvas.transform);
    }

    private void MiniGameInteraction()
    {
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
                gameChangeInformation.ChangeToFlowerGame();
                break;
            default:
                Debug.LogError("Mini game was activated but no mini game was chosen");
                break;
        }
    }
}
