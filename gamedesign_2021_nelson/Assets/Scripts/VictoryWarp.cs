using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryWarp : MonoBehaviour
{
    // The ID of the next room
    [SerializeField]
    int nextRoomId;

    // When colliding with the BoxCollider2D trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object colliding the victory warp trigger is a player
        if (collision.gameObject.tag == "Player") 
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            // Save the rest of the variables here because of possible abuse mechanisms
            PlayerPrefs.SetFloat("PlayerHealth", playerScript.playerHealth);
            PlayerPrefs.SetInt("ConsumableSelection", playerScript.consumableSelection);
            PlayerPrefs.SetInt("ShurikenAmount", playerScript.currentShuriken);
            PlayerPrefs.SetInt("BombAmount", playerScript.currentBombs);
            PlayerPrefs.SetInt("HealthPotionAmount", playerScript.currentHealthPotions);
            PlayerPrefs.SetInt("YellowKeyCount", playerScript.yellowCount);
            PlayerPrefs.SetInt("BlueKeyCount", playerScript.blueCount);
            PlayerPrefs.SetInt("RedKeyCount", playerScript.redCount);
            PlayerPrefs.SetInt("ActiveSceneID", SceneManager.GetActiveScene().buildIndex); 

            // Load next room
            SceneManager.LoadScene(nextRoomId);
        }
    }
}