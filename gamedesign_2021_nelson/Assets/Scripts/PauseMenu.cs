using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("ActiveSceneID", SceneManager.GetActiveScene().buildIndex);
        PlayerController playerScript = player.GetComponent<PlayerController>();
        PlayerPrefs.SetFloat("PlayerHealth", playerScript.playerHealth);
        PlayerPrefs.SetInt("ConsumableSelection", playerScript.consumableSelection);
        PlayerPrefs.SetInt("ShurikenAmount", playerScript.currentShuriken);
        PlayerPrefs.SetInt("BombAmount", playerScript.currentBombs);
        PlayerPrefs.SetInt("HealthPotionAmount", playerScript.currentHealthPotions);
        PlayerPrefs.SetInt("YellowKeyCount", playerScript.yellowCount);
        PlayerPrefs.SetInt("BlueKeyCount", playerScript.blueCount);
        PlayerPrefs.SetInt("RedKeyCount", playerScript.redCount);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("ActiveSceneID", SceneManager.GetActiveScene().buildIndex);
        PlayerController playerScript = player.GetComponent<PlayerController>();
        PlayerPrefs.SetFloat("PlayerHealth", playerScript.playerHealth);
        PlayerPrefs.SetInt("ConsumableSelection", playerScript.consumableSelection);
        PlayerPrefs.SetInt("ShurikenAmount", playerScript.currentShuriken);
        PlayerPrefs.SetInt("BombAmount", playerScript.currentBombs);
        PlayerPrefs.SetInt("HealthPotionAmount", playerScript.currentHealthPotions);
        PlayerPrefs.SetInt("YellowKeyCount", playerScript.yellowCount);
        PlayerPrefs.SetInt("BlueKeyCount", playerScript.blueCount);
        PlayerPrefs.SetInt("RedKeyCount", playerScript.redCount);

        Application.Quit();
    }

    public void LoadSave()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}