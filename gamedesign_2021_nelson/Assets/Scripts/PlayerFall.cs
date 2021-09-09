using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFall : MonoBehaviour
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
    }
}