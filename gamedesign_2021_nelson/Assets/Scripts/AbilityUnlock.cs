using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    // The parent object of this ability unlock
    GameObject abilityUnlockObject;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent variable
        abilityUnlockObject = transform.parent.gameObject;
    }

    // When an object enters the ability object trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If the triggering object is a player
        if (collision.gameObject.tag == "Player") 
        {
            // Enable doublejump and save it in PlayerPrefs
            collision.gameObject.GetComponent<PlayerController>().playerMaxJumpCounter = 2;
            PlayerPrefs.SetInt("DoubleJumpUnlocked", 2);

            // Disable this ability unlock object
            abilityUnlockObject.SetActive(false);
        }
    }
}