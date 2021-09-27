using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    PlayerController Loader;

    void Start()
    {
        Loader = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start a new game
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Load an existing game
    public void LoadGame()
    {
        Loader.LoadPlayer();
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}