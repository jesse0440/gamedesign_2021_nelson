using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Game variables that need to be saved for when the game is loaded next
    public float savedPlayerHealth;
    public float[] savedPlayerPosition;

    // Player object and script
    GameObject playerObject;
    PlayerController playerController;

    void Start()
    {
        // Assign the player object and script
        playerObject = GameObject.FindWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();
    }

    // This data will be saved
    public PlayerData()
    {
        //Saved data variables
        savedPlayerHealth = playerController.playerHealth;
        savedPlayerPosition = new float[2];
        savedPlayerPosition[0] = playerObject.transform.position.x;
        savedPlayerPosition[1] = playerObject.transform.position.y;
    }
}