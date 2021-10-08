using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int SceneID;

    AudioSource audio;

    void Start()
    {
        audio = GameObject.FindWithTag("GameManager").GetComponent<AudioSource>();
    }

    // Start a new game
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        audio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Load an existing game
    public void LoadGame()
    {
        SceneID = PlayerPrefs.GetInt("ActiveSceneID", SceneManager.GetActiveScene().buildIndex + 1);
        audio.Play();
        SceneManager.LoadScene(SceneID);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}