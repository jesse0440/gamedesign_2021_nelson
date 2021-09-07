using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player statistics
    public float player_speed = 7f;
    public float player_jump_height = 7.5f;
    public float player_jump_counter = 0;

    // Player components
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the Horizontal input of Input manager
        float horizontalDirection = Input.GetAxis("Horizontal");

        // The player movement
        rb.velocity = new Vector2(horizontalDirection * player_speed, rb.velocity.y);

        // While moving right
        if (horizontalDirection > 0) 
        {
            sprite.flipX = false;
        }

        // While moving left
        if (horizontalDirection < 0)
        {
            sprite.flipX = true;
        }

        // Jumping up
        if (Input.GetKeyDown(KeyCode.Space) && player_jump_counter < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, player_jump_height);
            player_jump_counter++;
        }

        // Resetting the jump counter when player hits the ground
        if (player_jump_counter < 0 || player_jump_counter >= 2 && bc.IsTouchingLayers())
        {
            player_jump_counter = 0;
        }
    }
}