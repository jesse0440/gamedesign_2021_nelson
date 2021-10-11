using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlatform : MonoBehaviour
{
    // The collider variables
    EdgeCollider2D playerEdgeCollider;
    BoxCollider2D boxCollider;

    bool phaseCheck = false;

    // Start is called before the first frame update
    void Awake()
    {
        // Assign the platform's collider
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update called once per frame
    void Update()
    {
        // Update player collider
        playerEdgeCollider = GameObject.FindWithTag("Player").GetComponent<EdgeCollider2D>();

        // If the player is under the phaseplatform unsolidify the platform
        if (playerEdgeCollider.bounds.min.y < boxCollider.bounds.max.y)
        {
            boxCollider.enabled = false;
            phaseCheck = false;
        }

        // If the player is above the phaseplatform solidify the platform
        if (playerEdgeCollider.bounds.min.y > boxCollider.bounds.max.y && phaseCheck == false)
        {
            boxCollider.enabled = true;
            phaseCheck = true;
        }

        // If the player is standing on the platform and they press any Vertical keys
        // Unsolidify the platform
        if (boxCollider.IsTouching(playerEdgeCollider) && Input.GetButtonDown("Phase"))
        {
            boxCollider.enabled = false;
        }
    }
}