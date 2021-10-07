using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Variable for determining an unique instance
    GameObject gameManagerInstance;

    // Check if enemies are allowed to spawn
    [HideInInspector]
    public bool canEnemiesSpawn = false;

    // Wake up protocols
    void Awake()
    {
        // This gameobject will never be destroyed when switching scenes/rooms
        DontDestroyOnLoad(gameObject);

        // If there is no instance of GameManager, assign this variable. Otherwise destroy the instance of GameManager.
        if (!gameManagerInstance)
        {
            gameManagerInstance = gameObject;
        } 
        
        else 
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("EnemiesCanSpawn") == 1)
        {
            canEnemiesSpawn = true;
        }

        else
        {
            canEnemiesSpawn = false;
        }
    }

    // When the application is closed, erase all checkpoints' progress in rooms
    // This is for testing purposes and will be removed later
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}