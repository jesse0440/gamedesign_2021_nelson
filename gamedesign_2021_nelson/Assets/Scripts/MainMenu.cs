using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //starts new game from next scene in load order
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //guits the game when quti is pressed
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
