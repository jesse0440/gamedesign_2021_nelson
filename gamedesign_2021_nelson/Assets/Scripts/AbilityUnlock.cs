using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    // The parent object of this ability unlock
    GameObject abilityUnlockObject;

    // The ID of the ability unlocked by this object
    // Must be entered manually in editor!!
    [SerializeField]
    int abilityID;

    // The value this ability adds to the player
    // Must be entered manually in editor!!
    [SerializeField]
    int abilityValue;

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
            // Enable ability based on the manual ID
            switch (abilityID)
            {
                case 0:
                    collision.gameObject.GetComponent<PlayerController>().playerMaxJumpCounter = 2;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            // Enable ability and save it in PlayerPrefs
            PlayerPrefs.SetInt("Ability_" + abilityID, abilityValue);

            // Disable this ability unlock object
            abilityUnlockObject.SetActive(false);
        }
    }
}