using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointLocation : MonoBehaviour
{
    // Variables needed
    float respawnXCoordinate;
    float respawnYCoordinate;

    void Awake()
    {
        // Check if the player has passed a respawn or use default
        respawnXCoordinate = PlayerPrefs.GetFloat("SpawnPointX", gameObject.transform.position.x);
        respawnYCoordinate = PlayerPrefs.GetFloat("SpawnPointY", gameObject.transform.position.y);

        // Move the spawnpoint
        gameObject.transform.position = new Vector3(respawnXCoordinate, respawnYCoordinate, 0);
    }
}