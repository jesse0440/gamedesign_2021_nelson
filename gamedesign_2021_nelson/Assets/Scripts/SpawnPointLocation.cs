using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointLocation : MonoBehaviour
{
    // Variables needed
    float respawnXCoordinate;
    float respawnYCoordinate;

    void Awake()
    {
        // Find the index of the active scene
        int sceneID = SceneManager.GetActiveScene().buildIndex;

        // Check if the player has passed a respawn or use default
        respawnXCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate X", gameObject.transform.position.x);
        respawnYCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate Y", gameObject.transform.position.y);

        /*
        PlayerData data = SaveSystem.LoadPlayer();
        
        if (sceneID != data.savedSceneNumber)
        {
            // Check if the player has passed a respawn or use default
            respawnXCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate X", gameObject.transform.position.x);
            respawnYCoordinate = PlayerPrefs.GetFloat("Room " + sceneID + " Respawn Coordinate Y", gameObject.transform.position.y);
        }



        else
        {
            Vector2 position;
            position.x = data.savedPlayerPosition[0];
            position.y = data.savedPlayerPosition[1];

            respawnXCoordinate = position.x;
            respawnYCoordinate = position.y;
        }
        */


        // Move the spawnpoint
        gameObject.transform.position = new Vector3(respawnXCoordinate, respawnYCoordinate, 0);
    }
}