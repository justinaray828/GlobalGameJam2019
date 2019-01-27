using System.Collections;
using System.Collections.Generic;
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
    private ParticleSystem ParticleSys;
    private MeshCollider Collider;

    private void Start()
    {
        gameChangeInformation = GameObject.FindGameObjectWithTag(GAMECHANGEINFORMATIONTAG).GetComponent<GameChangeInformation>();
        imageClueCanvas = GameObject.FindGameObjectWithTag(IMAGECLUETAG);
        GameObject particleSystem = GameObject.Instantiate(ParticleSystemPrefab);
        ParticleSys = particleSystem.GetComponent<ParticleSystem>();
        particleSystem.transform.parent = ParticleSystemAttachmentPoint;
        particleSystem.transform.localPosition = Vector3.zero;
        particleSystem.transform.localRotation = Quaternion.identity;

        Collider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        Vector3 playerPosition = PlayerMovement.Instance.transform.position;

        if (Vector3.Distance(playerPosition, Collider.bounds.center) <= 10f)
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

    /// <summary>
    /// Interact with clicked GameObject
    /// </summary>
    public void Interact()
    {
        switch(interactableObjectData.interactableObjectType)
        {
            case InteractableObjectData.InteractableObjectType.Text:
                TextInteraction();
                break;
            case InteractableObjectData.InteractableObjectType.Image:
                ImageInteraction();
                break;
            case InteractableObjectData.InteractableObjectType.MiniGame:
                MiniGameInteraction();
                break;
        }
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
