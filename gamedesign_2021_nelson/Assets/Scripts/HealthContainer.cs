using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthContainer : MonoBehaviour
{
    // The parent object of this health container
    GameObject healthParentObject;
    
    // The amount of health this object replenishes
    [SerializeField]
    float amountOfHealthGiven;

    // The ID of the container
    // Used to differentiate multiple containers in a room
    public int containerIDInRoom;

    // The ID of the room the container is located in
    int roomID;

    // Check if this container has already been consumed, default is 0 = false
    int hasThisContainerBeenUsedAlready = 0;

    // The script of the player character
    PlayerController playerController;

    AudioSource gameAudioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the parent object and the room ID variables
        healthParentObject = transform.parent.gameObject;
        roomID = SceneManager.GetActiveScene().buildIndex;

        gameAudioManager = GameObject.FindWithTag("GameAudioManager").GetComponent<AudioSource>();

        // Find out if this health container has already been used in this playthrough or use default (0)
        hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("HealthContainer" + roomID + "_" + containerIDInRoom, 0);

        // If it has been used
        if (hasThisContainerBeenUsedAlready == 1)
        {
            // Disable this container when re-entering the room
            healthParentObject.SetActive(false);
        }

        // Checking variables that should not be 0
        // Add as needed

        if (amountOfHealthGiven == 0f)
        {
            amountOfHealthGiven = 25f;
        }
    }

    // When an object enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object is the player and the container has not been consumed
        if (collision.gameObject.tag == "Player" && hasThisContainerBeenUsedAlready == 0)
        {
            // Assign PlayerController and increase the health
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.playerHealth += amountOfHealthGiven;

            // Play heart get sound
            gameAudioManager.clip = gameAudioManager.gameObject.GetComponent<GameAudioManager>().heartGet;
            gameAudioManager.Play();

            // Make the game remember that this container by ID, in this room by ID, has been consumed
            PlayerPrefs.SetInt("HealthContainer" + roomID + "_" + containerIDInRoom, 1);

            // Assign the check so the health container can't be consumed multiple times in a few frames before it disappears
            hasThisContainerBeenUsedAlready = PlayerPrefs.GetInt("HealthContainer" + roomID + "_" + containerIDInRoom, 0);

            // Disable the container
            healthParentObject.SetActive(false);
        }
    }
}