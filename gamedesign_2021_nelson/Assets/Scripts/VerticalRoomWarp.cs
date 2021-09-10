using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class VerticalRoomWarp : MonoBehaviour
{
    // The ID of the next room
    // Needs to be entered manually in the editor for each vertical warp object!!
    [SerializeField]
    int nextRoomId;

    // When entering the trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If object passing the room trigger is a player
        if (collision.gameObject.tag == "Player") 
        {
            // Find the index of the active scene and save the player's coordinates
            int temp = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetFloat("Room " + temp + " X Coordinate", collision.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("Room " + temp + " Y Coordinate", collision.gameObject.transform.position.y);
        }
    }

    // When colliding with the doorway
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If object colliding the doorway is a player
        if (collision.gameObject.tag == "Player") 
        {
            // Load next room
            SceneManager.LoadScene(nextRoomId);
        }
    }
}