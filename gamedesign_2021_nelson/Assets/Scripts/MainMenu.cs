using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int SceneID;

    // Start a new game
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Load an existing game
    public void LoadGame()
    {
        SceneID = PlayerPrefs.GetInt("ActiveSceneID");
        SceneManager.LoadScene(SceneID);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}