using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointLocation : MonoBehaviour
{
    // Variables needed
    public float respawnXCoordinate;
    public float respawnYCoordinate;

    void Awake()
    {
        // Find the index of the active scene
        int sceneID = SceneManager.GetActiveScene().buildIndex;

        // Check if the player has passed a respawn or use default
        respawnXCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate X", gameObject.transform.position.x);
        respawnYCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate Y", gameObject.transform.position.y);

        // Move the spawnpoint
        gameObject.transform.position = new Vector3(respawnXCoordinate, respawnYCoordinate, 0);
    }
}