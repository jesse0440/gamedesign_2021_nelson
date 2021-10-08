using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    AudioSource audio;

    void Start()
    {
        audio = GameObject.FindWithTag("GameManager").GetComponent<AudioSource>();
    }

    // Go back to the main menu
    public void MainMenu()
    {
        audio.Stop();
        SceneManager.LoadScene(0);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}