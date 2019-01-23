using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int currentSceneIndex;
    [SerializeField] private int MainMenuSceneIndex = 0;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
