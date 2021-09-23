using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShurikenContainer : MonoBehaviour
{
    // The parent object of this shuriken container
    GameObject shurikenParentObject;
    
    // The amount of shuriken this object replenishes
    // Needs to be entered manually in the editor!
    [SerializeField]
    float amountGiven = 10;

    // The ID of the container
    // Used to differentiate multiple containers in a room
    // Needs to be entered manually in the editor!
    [SerializeField]
    int shurikenContainerIDInRoom;

    // The ID of the room the container is located in
    int roomID;

    // Check if this container has already been consumed, default is 0 = false
    int hasThisContainerBeenUsedAlready = 0;

    // The script of the player character
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent object and the room ID variables
        shurikenParentObject = transform.parent.gameObject;
        roomID = SceneManager.GetActiveScene().buildIndex;

        // Find out if this container has already been used in this playthrough or use default (0)
        hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("ShurikenContainer" + roomID + "_" + shurikenContainerIDInRoom, 0);

        // If it has been used
        if (hasThisContainerBeenUsedAlready == 1)
        {
            // Disable this container when re-entering the room
            shurikenParentObject.SetActive(false);
        }

        // Checking variables that should not be 0
        // Add as needed

        if (amountGiven == 0f)
        {
            amountGiven = 25f;
        }
    }

    // When an object enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object is the player and the container has not been consumed
        if (collision.gameObject.tag == "Player" && hasThisContainerBeenUsedAlready == 0 )
        {
            // Assign PlayerController and Shuriken Count
            playerController = collision.gameObject.GetComponent<PlayerController>();
            //Check if  Player doesn't have max shuriken
            if (playerController.currentShuriken < playerController.maxShuriken){

                //make sure that currentShuriken doesn't go over the max
                playerController.currentShuriken += amountGiven;

                    if(playerController.currentShuriken > playerController.maxShuriken){
                        playerController.currentShuriken = playerController.maxShuriken;
                    }

                // Make the game remember that this container by ID, in this room by ID, has been consumed
                PlayerPrefs.SetInt("ShurikenContainer" + roomID + "_" + shurikenContainerIDInRoom, 1);

                // Assign the check so the health container can't be consumed multiple times in a few frames before it disappears
                hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("ShurikenContainer" + roomID + "_" + shurikenContainerIDInRoom, 0);

                // Disable the container
                shurikenParentObject.SetActive(false);
            }
            
        }
    }
}