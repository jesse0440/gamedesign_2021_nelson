using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPotionContainer : MonoBehaviour
{
    // The parent object of this health potion container
    GameObject healthPotionParentObject;
    
    // The amount of health this object replenishes
    [SerializeField]
    int amountGiven = 1;

    // The ID of the container
    // Used to differentiate multiple containers in a room
    public int containerIDInRoom;

    // The ID of the room the container is located in
    int roomID;

    // Check if this container has already been consumed, default is 0 = false
    int hasThisContainerBeenUsedAlready = 0;

    AudioSource gameAudioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent object and the room ID variables
        healthPotionParentObject = transform.parent.gameObject;
        roomID = SceneManager.GetActiveScene().buildIndex;

        gameAudioManager = GameObject.FindWithTag("GameAudioManager").GetComponent<AudioSource>();

        // Find out if this container has already been used in this playthrough or use default (0)
        hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("HealthPotionContainer" + roomID + "_" + containerIDInRoom, 0);
        
        // If it has been used
        if (hasThisContainerBeenUsedAlready == 1)
        {
            // Disable this container when re-entering the room
            healthPotionParentObject.SetActive(false);
        }

        // Checking variables that should not be 0
        // Add as needed

        if (amountGiven == 0)
        {
            amountGiven = 50;
        }
    }

    // When an object enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object is the player and the container has not been consumed
        if (collision.gameObject.tag == "Player" && hasThisContainerBeenUsedAlready == 0 )
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();

            // If the player has never obtained a health potion before
            if (PlayerPrefs.GetInt("HealthPotionObtained", 0) == 0)
            {
                // Allow health potions in the HUD
                PlayerPrefs.SetInt("HealthPotionObtained", 1);
                playerScript.healthPotionObtainedCheck = 1;
                playerScript.healthPotionIcon.SetActive(true);
            }

            // Add a health potion
            playerScript.currentHealthPotions += amountGiven;

            // Play consumable get sound
            gameAudioManager.clip = gameAudioManager.gameObject.GetComponent<GameAudioManager>().consumableGet;
            gameAudioManager.Play();

            // Make the game remember that this container by ID, in this room by ID, has been consumed
            PlayerPrefs.SetInt("HealthPotionContainer" + roomID + "_" + containerIDInRoom, 1);

            // Assign the check so the health potion container can't be consumed multiple times in a few frames before it disappears
            hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("HealthPotionContainer" + roomID + "_" + containerIDInRoom, 0);

            // Disable the container
            healthPotionParentObject.SetActive(false);
        }
    }
}