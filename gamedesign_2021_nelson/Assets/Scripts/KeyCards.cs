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

    // Global ID of a key
    [SerializeField]
    int globalKeyID;

    // The player's script
    PlayerController playerController;

    // Check to determine if this key was already given
    int keyGiven = 0;

    // Check to determine globally if key was already given
    int keyGiven2 = 0;

    // The ID of the room the container is located in
    int roomID;

    AudioSource gameAudioManager;

    void Start()
    {
        // Assign the player's script variable
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        roomID = SceneManager.GetActiveScene().buildIndex;

        gameAudioManager = GameObject.FindWithTag("GameAudioManager").GetComponent<AudioSource>();

        // Find out if this key has already been obtained in this playthrough or use default (0)
        keyGiven = PlayerPrefs.GetInt("Key" + roomID + "_" + keyIDInRoom, 0);
        keyGiven2 = PlayerPrefs.GetInt("GlobalKey_" + globalKeyID, 0);

        // If it has been obtained
        if (keyGiven == 1)
        {
            // Disable this key when re-entering the room
            gameObject.SetActive(false);
        }

        // If it has been obtained
        if (keyGiven2 == 1)
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
        if (collision.gameObject.tag == "Player" && keyGiven == 0 && keyGiven2 == 0)
        {
            // Give the player a key of this object's type and destroy this object
            playerController.AddKey(GetKeyType());

            // Play key get sound
            gameAudioManager.clip = gameAudioManager.gameObject.GetComponent<GameAudioManager>().keyGet;
            gameAudioManager.Play();

            // Make the game remember that this key by ID, in this room by ID, has been obtained
            PlayerPrefs.SetInt("Key" + roomID + "_" + keyIDInRoom, 1);

            // Make the game remember that this global key ID has been used to obtain a key
            PlayerPrefs.SetInt("GlobalKey_" + globalKeyID, 1);

            // Assign the check so keys can't be obtained multiple times in the few frames before disappearing
            keyGiven = PlayerPrefs.GetInt("Key" + roomID + "_" + keyIDInRoom, 0);
            keyGiven2 = PlayerPrefs.GetInt("GlobalKey_" + globalKeyID, 0);

            Destroy(gameObject);
        }
    }
}