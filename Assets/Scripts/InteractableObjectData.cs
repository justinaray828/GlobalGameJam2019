using UnityEngine;
using System.Collections;

public enum PictureType
{
    Snow, 
    Honeymoon, 
    Dancing, 
    Kid, 
    Baby, 
    Mountain, 
    Book
}

[CreateAssetMenu(fileName = "Data", menuName = "InteractableObjectData", order = 1)]
public class InteractableObjectData : ScriptableObject
{
    public enum InteractableObjectType { Text, Image, MiniGame };
    public InteractableObjectType interactableObjectType = InteractableObjectType.Text;
    public enum MiniGameType { NoGame, Dancing, Fridge, Flowers};
    public MiniGameType miniGameType = MiniGameType.NoGame;
    public string objectName = "Object Name";
    public SpeechBubbleSettings[] outputText = {};
    public GameObject clue; 
    public PictureType clueType;
}