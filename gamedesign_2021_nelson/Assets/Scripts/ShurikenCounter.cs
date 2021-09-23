using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenCounter : MonoBehaviour
{
    
    // Regular variables needed for the script
    GameObject player;
    float clonedCurrentShuriken;
    float clonedMaxShuriken;
    
    // When scene loads
    void Awake()
    {
        // Assign the player and its variables
        player = GameObject.FindWithTag("Player");
        clonedCurrentShuriken = player.GetComponent<PlayerController>().currentShuriken;
        clonedMaxShuriken = player.GetComponent<PlayerController>().maxShuriken;

    }

    // Update is called once per frame
    void Update()
    {

        // Update the new shuriken amount and check again
        gameObject.GetComponent<Text>().text = clonedCurrentShuriken + " / " + clonedMaxShuriken;
        clonedCurrentShuriken = player.GetComponent<PlayerController>().currentShuriken;
        
    }
}
