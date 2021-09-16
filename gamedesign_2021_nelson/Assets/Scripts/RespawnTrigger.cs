using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    // Place these triggers near each entrance to a room (mandatory) and in the middle somewhere if needed
    // This object's collider
    BoxCollider2D boxCollider;

    // The spawnpoint of the room this object is in
    Transform spawnPointLocation;

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
            // Move the spawnpoint of the room to this respawn point trigger
            PlayerPrefs.SetFloat("SpawnPointX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("SpawnPointY", gameObject.transform.position.y);

            // Disable this respawn point trigger
            gameObject.SetActive(false);
        }
    }
}