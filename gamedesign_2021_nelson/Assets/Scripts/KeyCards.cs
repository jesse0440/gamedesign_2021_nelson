using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCards : MonoBehaviour
{
    // Below is the option to choose what colour the keycard is in the editor with default being yellow
    // A key can only be used on a door of the same colour
    [SerializeField] 
    KeyType keyType;

    // The player's script
    PlayerController playerController;

    // Bool to determine if this key was already given
    bool keyGiven = false;

    void Start()
    {
        // Assign the player's script variable
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
        if (collision.gameObject.tag == "Player" && keyGiven == false)
        {
            // Give the player a key of this object's type and destroy this object
            playerController.AddKey(GetKeyType());
            keyGiven = true;
            Destroy(gameObject);
        }
    }
}