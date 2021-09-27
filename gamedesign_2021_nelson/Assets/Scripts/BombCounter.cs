using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombCounter : MonoBehaviour
{
    // Regular variables needed for the script
    GameObject player;
    float clonedCurrentBombs;
    float clonedMaxBombs;
    
    // When scene loads
    void Awake()
    {
        // Assign the player and its variables
        player = GameObject.FindWithTag("Player");
        clonedCurrentBombs = player.GetComponent<PlayerController>().currentBombs;
        clonedMaxBombs = player.GetComponent<PlayerController>().maxBombs;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has ever obtained bombs
        if (player.GetComponent<PlayerController>().bombObtainedCheck == 1)
        {
            // Update the new bomb amount and check again
            gameObject.GetComponent<Text>().text = clonedCurrentBombs + " / " + clonedMaxBombs;
            clonedCurrentBombs = player.GetComponent<PlayerController>().currentBombs;
        }
    }
}