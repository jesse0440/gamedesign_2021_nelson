using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyDoor : MonoBehaviour
{
    //Choose the key used for this foor, default is yellow
    [SerializeField] 
    KeyCards.KeyType keyType;

    // The ID of the door
    // Used to differentiate multiple doors in a room
    [SerializeField]
    int doorIDInRoom;

    // The player's script
    PlayerController playerController;

    // Check to determine if a key was already used
    int keyUsed = 0;

    // The ID of the room the container is located in
    int roomID;

    AudioSource gameAudioManager;

    void Start()
    {
        // Assign the player's script variable
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        roomID = SceneManager.GetActiveScene().buildIndex;

        gameAudioManager = GameObject.FindWithTag("GameAudioManager").GetComponent<AudioSource>();

        // Find out if this door has already been opened in this playthrough or use default (0)
        keyUsed = PlayerPrefs.GetInt("Door" + roomID + "_" + doorIDInRoom, 0);

        // If it has been opened
        if (keyUsed == 1)
        {
            // Disable this door when re-entering the room
            gameObject.SetActive(false);
        }
    }

    // When called return which key type this door uses
    public KeyCards.KeyType GetKeyType()
    {
        return keyType;
    }

    // If something enters this door's trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is the player
        if (collision.gameObject.tag == "Player" && keyUsed == 0)
        {
            // If the player has the correct key
            if (playerController.ContainsKey(GetKeyType()))
            {
                // Remove one key of this door's type from the player
                playerController.RemoveKey(GetKeyType());

                // Play door opened sound
                gameAudioManager.clip = gameAudioManager.gameObject.GetComponent<GameAudioManager>().doorOpened;
                gameAudioManager.Play();
                
                // Make the game remember that this door by ID, in this room by ID, has been opened
                PlayerPrefs.SetInt("Door" + roomID + "_" + doorIDInRoom, 1);

                // Assign the check so keys can't be consumed multiple times in the few frames before door disappears
                keyUsed = PlayerPrefs.GetInt("Door" + roomID + "_" + doorIDInRoom, 0);

                OpenDoor();
            }
        }
    }

    // If the key type is right open the door
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}