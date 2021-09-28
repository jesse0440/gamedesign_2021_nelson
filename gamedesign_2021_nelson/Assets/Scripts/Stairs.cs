using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    // The collider variables
    GameObject player;
    EdgeCollider2D playerCollider;
    PolygonCollider2D collider;
    PolygonCollider2D collider2;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the player, the player's edge collider and the stairs' collider
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<EdgeCollider2D>();
        collider = gameObject.GetComponent<PolygonCollider2D>();
        collider2 = gameObject.GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (collider.IsTouching(playerCollider))
        {
            if (Input.GetButtonDown("Phase"))
            {
                collider.enabled = false;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButton("Phase"))
            {
                collider.enabled = false;
            }
        }
    }

    void OnTriggerExit2D()
    {
        collider = collider2;
    }
}