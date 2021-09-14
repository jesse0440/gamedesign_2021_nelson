using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlatform : MonoBehaviour
{
    // The collider variables
    GameObject player;
    EdgeCollider2D playerEdgeCollider;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        // Assign the player, the player's edge collider and the platform's collider
        player = GameObject.FindWithTag("Player");
        playerEdgeCollider = player.GetComponent<EdgeCollider2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update called once per frame
    void Update()
    {
        // If the player's Y position + the player's height / 2 is lower than the platform's Y position
        // Unsolidify the platform
        if (player.transform.position.y + 1.365f < boxCollider.transform.position.y)
        {
            boxCollider.enabled = false;
        }

        // If the player's Y position - the player's height / 2 is higher than the platform's Y position
        // Solidify the platform
        if (player.transform.position.y - 1.365f > boxCollider.transform.position.y)
        {
            boxCollider.enabled = true;
        }

        // If the player is standing on the platform and they press any Vertical keys
        // Unsolidify the platform
        if (boxCollider.IsTouching(playerEdgeCollider) && Input.GetButtonDown("Vertical"))
        {
            boxCollider.enabled = false;
        }
    }
}