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

    private void Start()
    {
        MainGame.SetActive(true);
        DancingGame.SetActive(false);
        FridgeGame.SetActive(false);
        FlowerGame.SetActive(false);
    }

    public void ChangeToMainGame()
    {
        FlowerGame.SetActive(false);
        FridgeGame.SetActive(false);
        DancingGame.SetActive(false);
        MainGame.SetActive(true);
    }

    public void ChangeToMainGame(bool pass)
    {
        if(pass)
        {

        }
        else
        {

        }
    }

    public void ChangeToDancingGame()
    {
        MainGame.SetActive(false);
        DancingGame.SetActive(true);
    }

    public void ChangeToFridgeGame()
    {
        MainGame.SetActive(false);
        FridgeGame.SetActive(true);
    }

    public void ChangeToFlowerGame()
    {
        MainGame.SetActive(false);
        FlowerGame.SetActive(true);
    }
}