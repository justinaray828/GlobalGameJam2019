using UnityEngine;
using System.Collections;

public class GameChangeInformation : ScriptableObject
{
    public GameObject MainGame;
    public GameObject DancingGame;
    public GameObject FridgeGame;
    public GameObject FlowerGame;

    public GameObject MainGameController;
    public GameObject DancingGameController;
    public GameObject FridgeGameController;
    public GameObject FlowerGameController;

    public Camera MainGameCamera;
    public Camera DancingGameCamera;
    public Camera FridgeGameCamera;
    public Camera FlowerGameCamera;

    public void ChangeToDancingGame()
    {
        MainGame.SetActive(false);
        MainGameController.SetActive(false);

        DancingGame.SetActive(true);
        DancingGameController.SetActive(true);
    }
}