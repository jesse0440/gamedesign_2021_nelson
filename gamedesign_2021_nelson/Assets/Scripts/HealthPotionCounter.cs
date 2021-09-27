using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotionCounter : MonoBehaviour
{
    // Regular variables needed for the script
    GameObject player;
    float clonedCurrentHealthPotions;
    float clonedMaxHealthPotions;
    
    // When scene loads
    void Awake()
    {
        // Assign the player and its variables
        player = GameObject.FindWithTag("Player");
        clonedCurrentHealthPotions = player.GetComponent<PlayerController>().currentHealthPotions;
        clonedMaxHealthPotions = player.GetComponent<PlayerController>().maxHealthPotions;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has ever obtained health potions
        if (player.GetComponent<PlayerController>().healthPotionObtainedCheck == 1)
        {
            // Update the new health potion amount and check again
            gameObject.GetComponent<Text>().text = clonedCurrentHealthPotions + " / " + clonedMaxHealthPotions;
            clonedCurrentHealthPotions = player.GetComponent<PlayerController>().currentHealthPotions;
        }
    }
}