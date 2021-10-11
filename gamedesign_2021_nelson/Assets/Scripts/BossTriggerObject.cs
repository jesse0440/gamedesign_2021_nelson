using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerObject : MonoBehaviour
{
    // Set variables
    BoxCollider2D boxCollider;
    GameObject gameManager;
    AudioSource gameAudioManager;
    AudioSource gameAudio;
    GameObject[] bossWalls;
    public GameObject victoryWarp;
    int bossBeenFoughtValue = 0;

    // ID of the boss this trigger is used for
    [SerializeField]
    int bossID;

    // Start is called before the first frame update
    void Start()
    {
        // Check if boss has already been fought
        bossBeenFoughtValue = PlayerPrefs.GetInt("BossFought_" + bossID, 0);

        if (bossBeenFoughtValue == 1)
        {
            gameObject.SetActive(false);
        }

        // Assign variables
        boxCollider = GetComponent<BoxCollider2D>();
        gameManager = GameObject.FindWithTag("GameManager");
        gameAudioManager = GameObject.FindWithTag("GameAudioManager").GetComponent<AudioSource>();
        bossWalls = GameObject.FindGameObjectsWithTag("BossWall");
        victoryWarp = GameObject.FindWithTag("Victory");

        // Disable walls at first
        foreach (GameObject wall in bossWalls)
        {
            wall.SetActive(false);
        }
    }

    // When something enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is the player
        if (collision.gameObject.tag == "Player")
        {
            // If enemies can spawn
            if (gameManager.GetComponent<GameManagerScript>().canEnemiesSpawn == true)
            {
                // Find the walls and enable them
                foreach (GameObject wall in bossWalls)
                {
                    wall.SetActive(true);
                }

                // Play boss start sound
                gameAudioManager.clip = gameAudioManager.gameObject.GetComponent<GameAudioManager>().bossStart;
                gameAudioManager.Play();

                // Switch Audio
                gameManager.GetComponent<AudioSource>().clip = gameManager.GetComponent<GameManagerScript>().bossMusic;
                gameManager.GetComponent<AudioSource>().Play();

                gameObject.SetActive(false);
            }
        }
    }
}