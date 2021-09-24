using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyCards : MonoBehaviour
{
    // Below is the option to choose what colour the keycard is in the editor with default being yellow
    // A key can only be used on a door of the same colour
    [SerializeField] 
    KeyType keyType;

    // The ID of the key
    // Used to differentiate multiple keys in a room
    [SerializeField]
    int keyIDInRoom;

    // The player's script
    PlayerController playerController;

    // Check to determine if this key was already given
    int keyGiven = 0;

    // The ID of the room the container is located in
    int roomID;

    void Start()
    {
        // Assign the player's script variable
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        roomID = SceneManager.GetActiveScene().buildIndex;

        // Find out if this key has already been obtained in this playthrough or use default (0)
        keyGiven = PlayerPrefs.GetInt("Key" + roomID + "_" + keyIDInRoom, 0);

        // If it has been obtained
        if (keyGiven == 1)
        {
            // Disable this key when re-entering the room
            gameObject.SetActive(false);
        }
    }

    // Enum for different key types
    public enum KeyType
    {
        Yellow,
        Red,
        Blue
    }

    // Return the current key type
    public KeyType GetKeyType()
    {
        return keyType;
    }

    // If something enters this key's trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is the player
        if (collision.gameObject.tag == "Player" && keyGiven == 0)
        {
            // Give the player a key of this object's type and destroy this object
            playerController.AddKey(GetKeyType());

            // Make the game remember that this key by ID, in this room by ID, has been obtained
            PlayerPrefs.SetInt("Key" + roomID + "_" + keyIDInRoom, 1);

            // Assign the check so keys can't be obtained multiple times in the few frames before disappearing
            keyGiven = PlayerPrefs.GetInt("Key" + roomID + "_" + keyIDInRoom, 0);

            Destroy(gameObject);
        }
    }
}