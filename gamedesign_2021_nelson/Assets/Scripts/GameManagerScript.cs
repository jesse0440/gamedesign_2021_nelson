using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Variable for determining an unique instance
    public static GameObject gameManagerInstance;

    // Check if enemies are allowed to spawn
    [HideInInspector]
    public bool canEnemiesSpawn = false;
    public AudioClip gameMusic;
    public AudioClip enemyMusic;
    public AudioClip bossMusic;

    // Wake up protocols
    void Awake()
    {
        // If there is an instance of GameManager already destroy this one. Otherwise assign the instance of GameManager.
        if (gameManagerInstance)
        {
            DestroyImmediate(gameObject);
        } 
        
        else 
        {
            DontDestroyOnLoad(gameObject);
            gameManagerInstance = gameObject;
        }
    }

    // Start up protocols
    void Start()
    {
        if (PlayerPrefs.GetInt("EnemiesCanSpawn") == 1)
        {
            canEnemiesSpawn = true;
        }

        else
        {
            canEnemiesSpawn = false;
        }
    }
}