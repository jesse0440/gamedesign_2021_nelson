using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    // Needed when player obtain a health upgrade
    [HideInInspector]
    public bool maxHealthChanged;
    
    // Regular variables needed for the script
    GameObject player;
    Slider playerHealthBarSlider;
    float clonedPlayerHealth;
    float clonedPlayerMaxHealth;
    
    // When scene loads
    void Awake()
    {
        // Assign the player and its variables
        player = GameObject.FindWithTag("Player");
        clonedPlayerHealth = player.GetComponent<PlayerController>().playerHealth;
        clonedPlayerMaxHealth = player.GetComponent<PlayerController>().playerMaxHealth;

        // Assign the health bar HUD element to its variable
        playerHealthBarSlider = GameObject.FindWithTag("PlayerHealthBar").GetComponent<Slider>();
        playerHealthBarSlider.maxValue = clonedPlayerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar in the HUD if player health changes
        playerHealthBarSlider.value = clonedPlayerHealth;

        // Update the new health amount and check again
        gameObject.GetComponent<Text>().text = "Health " + clonedPlayerHealth + "/" + clonedPlayerMaxHealth;
        clonedPlayerHealth = player.GetComponent<PlayerController>().playerHealth;
        
        // If max health has risen
        if (maxHealthChanged)
        {
            clonedPlayerMaxHealth = player.GetComponent<PlayerController>().playerMaxHealth;
            maxHealthChanged = false;
        }
    }
}