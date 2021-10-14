using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    AudioSource audio;

    [HideInInspector]
    public int comicBeginning = 0;
    [HideInInspector]
    public int comicMiddle = 0;
    [HideInInspector]
    public int comicEnding = 0;
    [HideInInspector]
    public bool comicBCheck = false;
    [HideInInspector]
    public bool comicMCheck = false;
    [HideInInspector]
    public bool comicECheck = false;

    public bool secretBossBeaten = false;

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
        comicBeginning = PlayerPrefs.GetInt("ComicBeginning", 0);
        comicMiddle = PlayerPrefs.GetInt("ComicMiddle", 0);
        comicEnding = PlayerPrefs.GetInt("ComicEnding", 0);

        audio = GetComponent<AudioSource>();
        audio.loop = true;

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 14)
        {
            audio.Play();
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

    void Update()
    {
        if (comicBCheck == true)
        {
            comicBeginning = PlayerPrefs.GetInt("ComicBeginning", 0);
            comicBCheck = false;
        }

        if (comicMCheck == true)
        {
            comicMiddle = PlayerPrefs.GetInt("ComicMiddle", 0);
            comicMCheck = false;
        }

        if (comicECheck == true)
        {
            comicEnding = PlayerPrefs.GetInt("ComicEnding", 0);
            comicECheck = false;
        }
    }
}