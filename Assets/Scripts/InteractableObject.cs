using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractableObjectData interactableObjectData;
    [SerializeField] private Canvas SpeechCanvas;
    private GameChangeInformation gameChangeInformation;
    private const string GAMECHANGEINFORMATIONTAG = "GameChangeInformation";

    private void Start()
    {
        gameChangeInformation = GameObject.FindGameObjectWithTag(GAMECHANGEINFORMATIONTAG).GetComponent<GameChangeInformation>();
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

    }

    private void ImageInteraction()
    {

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
