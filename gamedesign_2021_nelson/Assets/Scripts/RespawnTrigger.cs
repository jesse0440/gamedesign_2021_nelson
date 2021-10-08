using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnTrigger : MonoBehaviour
{
    // Place these triggers near each entrance to a room (mandatory) and in the middle somewhere if needed
    // This object's collider
    BoxCollider2D boxCollider;

    // The spawnpoint of the room this object is in
    Transform spawnPointLocation;

    GameObject player;

    void Start()
    {
        // Assign the collider and the spawnpoint
        boxCollider = GetComponent<BoxCollider2D>();
        spawnPointLocation = GameObject.FindWithTag("SpawnPointLocation").transform;
    }

    // If something enters this respawn point trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If it is a player
        if (collision.gameObject.tag == "Player")
        {
            // Find the index of the active scene
            int sceneID = SceneManager.GetActiveScene().buildIndex;

            
            // Move the spawnpoint of the room to this respawn point trigger
            PlayerPrefs.SetFloat("Room " + sceneID + " Respawn Coordinate X", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("Room " + sceneID + " Respawn Coordinate Y", gameObject.transform.position.y);

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

            // Disable this respawn point trigger
            gameObject.SetActive(false);
        }
    }
}