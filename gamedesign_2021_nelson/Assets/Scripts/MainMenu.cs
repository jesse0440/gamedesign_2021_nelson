using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int SceneID;

    AudioSource audio;

    [SerializeField]
    GameObject comicBeginning;

    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject speedrun;

    GameManagerScript gameManagerScript;

    int speedRunMode = 0;

    void Start()
    {
        audio = GameObject.FindWithTag("GameManager").GetComponent<AudioSource>();
        gameManagerScript = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        speedRunMode = PlayerPrefs.GetInt("SpeedrunMode", 0);

        if (speedRunMode == 1)
        {
            speedrun.SetActive(true);
        }
    }

    // Start a new game
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        gameManagerScript.canEnemiesSpawn = false;
        gameManagerScript.comicBeginning = 0;
        gameManagerScript.comicMiddle = 0;
        gameManagerScript.comicEnding = 0;
        mainMenu.SetActive(false);
        comicBeginning.SetActive(true);
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

    public void ComicBeginning()
    {
        PlayerPrefs.SetInt("ComicBeginning", 1);
        gameManagerScript.comicBCheck = true;
        audio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SpeedrunMode()
    {
        PlayerPrefs.DeleteAll();
        gameManagerScript.canEnemiesSpawn = false;
        gameManagerScript.secretBossBeaten = true;
        gameManagerScript.comicBeginning = 0;
        gameManagerScript.comicMiddle = 0;
        gameManagerScript.comicEnding = 0;
        mainMenu.SetActive(false);
        comicBeginning.SetActive(true);
    }
}