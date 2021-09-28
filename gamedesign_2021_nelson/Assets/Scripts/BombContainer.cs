using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombContainer : MonoBehaviour
{
    // The parent object of this bomb container
    GameObject bombParentObject;
    
    // The amount of bomb this object replenishes
    [SerializeField]
    int amountGiven = 5;

    // The ID of the container
    // Used to differentiate multiple containers in a room
    public int containerIDInRoom;

    // The ID of the room the container is located in
    int roomID;

    // Check if this container has already been consumed, default is 0 = false
    int hasThisContainerBeenUsedAlready = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent object and the room ID variables
        bombParentObject = transform.parent.gameObject;
        roomID = SceneManager.GetActiveScene().buildIndex;

        // Find out if this container has already been used in this playthrough or use default (0)
        hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("BombContainer" + roomID + "_" + containerIDInRoom, 0);
        
        // If it has been used
        if (hasThisContainerBeenUsedAlready == 1)
        {
            // Disable this container when re-entering the room
            bombParentObject.SetActive(false);
        }

        // Checking variables that should not be 0
        // Add as needed

        if (amountGiven == 0)
        {
            amountGiven = 5;
        }
    }

    // When an object enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object is the player and the container has not been consumed
        if (collision.gameObject.tag == "Player" && hasThisContainerBeenUsedAlready == 0 )
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();

            // If the player has never obtained bombs before
            if (PlayerPrefs.GetInt("BombObtained", 0) == 0)
            {
                // Allow bombs in the HUD
                PlayerPrefs.SetInt("BombObtained", 1);
                playerScript.bombObtainedCheck = 1;
                playerScript.bombIcon.SetActive(true);
            }

            // Add bombs
            playerScript.currentBombs += amountGiven;

            // Make the game remember that this container by ID, in this room by ID, has been consumed
            PlayerPrefs.SetInt("BombContainer" + roomID + "_" + containerIDInRoom, 1);

            // Assign the check so the bomb container can't be consumed multiple times in a few frames before it disappears
            hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("BombContainer" + roomID + "_" + containerIDInRoom, 0);

            // Disable the container
            bombParentObject.SetActive(false);
        }
    }
}