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

    private void Start()
    {
        MainGame.SetActive(true);
        DancingGame.SetActive(false);
        FridgeGame.SetActive(false);
        FlowerGame.SetActive(false);
    }

    public void ChangeToMainGame(bool pass)
    {
        FlowerGame.SetActive(false);
        FridgeGame.SetActive(false);
        DancingGame.SetActive(false);
        MainGame.SetActive(true);

        if (pass)
        {
            FindObjectOfType<AudioManager>().ToHomeMusicOnSuccess();
            Debug.Log("reached success");
            //TODO: insert flavor text for both success and failure.
        }
        else
        {
            FindObjectOfType<AudioManager>().ToHomeMusicOnFailure();
            Debug.Log("Reached Fail State");
        }
    }

    public void ChangeToDancingGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToDanceMusic();
        DancingGame.SetActive(true);
    }

    public void ChangeToFridgeGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToFridgeMusic();
        FridgeGame.SetActive(true);
    }

    public void ChangeToFlowerGame()
    {
        MainGame.SetActive(false);
        FindObjectOfType<AudioManager>().ToFlowerMusic();
        FlowerGame.SetActive(true);
    }
}