using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallZone : MonoBehaviour
{
    // When the player enters the invisible barrier under the level
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If the object which fell was a player
        if (collision.gameObject.tag == "Player") 
        {
            // Load the level again = Respawn
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // If the object which fell was an enemy
        else if (collision.gameObject.tag == "Enemy")
        {
            // Destroy the enemy
            Destroy(collision.gameObject);
        }
    }
}