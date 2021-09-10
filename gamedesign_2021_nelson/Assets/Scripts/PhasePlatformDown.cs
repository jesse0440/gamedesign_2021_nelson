using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlatformDown : MonoBehaviour
{
    // The collider variables
    PolygonCollider2D playerPolygonCollider;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the player's collider and the platform's collider
        playerPolygonCollider = GameObject.FindWithTag("Player").GetComponent<PolygonCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // If a player is standing on the platform and you press S or Down Arrow, disable the platform's box collider
        if (boxCollider.IsTouching(playerPolygonCollider) && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            boxCollider.enabled = false;
        }
    }
}