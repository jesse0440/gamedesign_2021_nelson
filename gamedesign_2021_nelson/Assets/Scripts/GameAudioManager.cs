using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Variable for determining an unique instance
    public static GameObject gameAudioManagerInstance;

    [Header("Player Audio")]
    public AudioClip playerJumping;
    public AudioClip playerSlashing;
    public AudioClip playerThrowing;
    public AudioClip playerDrinking;
    public AudioClip playerDash;
    public AudioClip playerTeleport;
    public AudioClip playerHit;
    public AudioClip playerDeath;

    [Header("Enemy Audio")]
    public AudioClip enemyNoticesPlayer;
    public AudioClip enemySpawning;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip bossShooting;
    public AudioClip bossHit;
    public AudioClip bossDeath;

    [Header("Level Audio")]
    public AudioClip keyGet;
    public AudioClip doorOpened;
    public AudioClip abilityGet;
    public AudioClip consumableGet;
    public AudioClip heartGet;

    [Header("Miscellaneous Audio")]
    public AudioClip explosion;
    public AudioClip victory;
    public AudioClip bossStart;

    AudioSource audio;

    // Wake up protocols
    void Awake()
    {
        // If there is an instance of GameAudioManager already destroy this one. Otherwise assign the instance of GameAudioManager.
        if (gameAudioManagerInstance)
        {
            DestroyImmediate(gameObject);
        } 
        
        else 
        {
            DontDestroyOnLoad(gameObject);
            gameAudioManagerInstance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }
}