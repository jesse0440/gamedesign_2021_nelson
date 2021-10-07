using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerObject : MonoBehaviour
{
    // Set variables
    BoxCollider2D boxCollider;
    GameObject gameManager;
    GameObject[] bossWalls;
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
        bossWalls = GameObject.FindGameObjectsWithTag("BossWall");

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
            }
        }
    }
}