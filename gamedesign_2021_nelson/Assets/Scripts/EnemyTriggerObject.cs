using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerObject : MonoBehaviour
{
    // Set variables
    BoxCollider2D boxCollider;
    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        gameManager = GameObject.FindWithTag("GameManager");
    }

    // When something enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is the player
        if (collision.gameObject.tag == "Player")
        {
            // Enable enemy spawns throughtout the map
            gameManager.GetComponent<GameManagerScript>().canEnemiesSpawn = true;
            PlayerPrefs.SetInt("EnemiesCanSpawn", 1);
        }
    }
}