using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // The ID of the room the ability unlock is located in
    int roomID;

    // Check if this ability unlock has already been used, default is 0 = false
    int hasThisAbilityUnlockBeenUsedAlready = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent object and the room ID variables
        abilityUnlockObject = transform.parent.gameObject;
        roomID = SceneManager.GetActiveScene().buildIndex;

        // Find out if this ability unlock has already been used in this playthrough or use default (0)
        hasThisAbilityUnlockBeenUsedAlready = PlayerPrefs.GetInt("AbilityUnlock" + roomID + "_" + abilityID, 0);

        // If it has been used
        if (hasThisAbilityUnlockBeenUsedAlready == 1)
        {
            // Disable this ability unlock when re-entering the room
            abilityUnlockObject.SetActive(false);
        }
    }

    // When an object enters the ability object trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // If the triggering object is a player
        if (collision.gameObject.tag == "Player" && hasThisAbilityUnlockBeenUsedAlready == 0) 
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

            PlayerPrefs.SetInt("AbilityUnlock" + roomID + "_" + abilityID, 1);

            hasThisAbilityUnlockBeenUsedAlready = PlayerPrefs.GetInt("AbilityUnlock" + roomID + "_" + abilityID, 0);

            // Enable ability and save it in PlayerPrefs
            PlayerPrefs.SetInt("Ability_" + abilityID, abilityValue);

            // Disable this ability unlock object
            abilityUnlockObject.SetActive(false);
        }
    }
}