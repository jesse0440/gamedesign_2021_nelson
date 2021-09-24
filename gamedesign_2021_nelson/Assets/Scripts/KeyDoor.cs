using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    //Choose the key used for this foor, default is yellow
    [SerializeField] 
    KeyCards.KeyType keyType;

    // The player's script
    PlayerController playerController;

    // Bool to determine if a key was already used
    bool keyUsed = false;

    void Start()
    {
        // Assign the player's script variable
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
        if (collision.gameObject.tag == "Player" && keyUsed == false)
        {
            if (playerController.ContainsKey(GetKeyType()))
            {
                playerController.RemoveKey(GetKeyType());
                keyUsed = true;
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