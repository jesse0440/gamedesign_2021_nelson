using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    AudioSource audio;

    [SerializeField]
    GameObject comicEnding;

    [SerializeField]
    GameObject victoryMenu;

    GameManagerScript gameManagerScript;

    void Start()
    {
        audio = GameObject.FindWithTag("GameManager").GetComponent<AudioSource>();
        gameManagerScript = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        audio.Stop();
    }

    // Go back to the main menu
    public void MainMenu()
    {
        //audio.Stop();
        SceneManager.LoadScene(0);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ComicEnding()
    {
        PlayerPrefs.SetInt("ComicEnding", 1);
        gameManagerScript.comicECheck = true;
        comicEnding.SetActive(false);
        victoryMenu.SetActive(true);
    }
}