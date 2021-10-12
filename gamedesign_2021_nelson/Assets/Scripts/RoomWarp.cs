using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class RoomWarp : MonoBehaviour
{
    // PolygonCollider2D saves the coordinates of your character's position from when you last visited the room
    // BoxCollider2D is the collider which actually warps you
    // In short: always position the PolygonCollider2D before the BoxCollider2D

    // The ID of the next room
    [SerializeField]
    int nextRoomId;

    GameObject player;

    // When entering the PolygonCollider2D trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If the object passing the room trigger is a player
        if (collision.gameObject.tag == "Player") 
        {
            // Find the index of the active scene and save the necessary variables
            int sceneID = SceneManager.GetActiveScene().buildIndex;

            PlayerPrefs.SetFloat("Room " + sceneID + " X Coordinate", collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("Room " + sceneID + " Y Coordinate", collision.gameObject.transform.position.y);
            
            /*
            player = GameObject.FindWithTag("Player");
            SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
            */
        }
    }

    // When colliding with the BoxCollider2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the object colliding the warp collider is a player
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