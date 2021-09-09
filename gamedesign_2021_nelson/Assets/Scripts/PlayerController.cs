using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Player statistics
    public float player_health = 100f;
    public float player_max_health = 100f;
    public float player_speed = 7f;
    public float player_jump_height = 7.5f;
    public float player_jump_counter = 0;
    public float player_max_jump_counter = 2;

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
        // If the player's health drops to zero or bugs out otherwise
        if (player_health > player_max_health || player_health <= 0) 
        {
            // Respawn the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Get the Horizontal input of Input manager
        float horizontalDirection = Input.GetAxis("Horizontal");

        // The player movement
        rb.velocity = new Vector2(horizontalDirection * player_speed, rb.velocity.y);

        // While moving right
        if (horizontalDirection > 0) 
        {
            // Flip sprite to face right
            sprite.flipX = false;
        }

        // While moving left
        if (horizontalDirection < 0)
        {
            // Flip sprite to face left
            sprite.flipX = true;
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetKeyDown(KeyCode.Space) && player_jump_counter < player_max_jump_counter)
        {
            rb.velocity = new Vector2(rb.velocity.x, player_jump_height);
            player_jump_counter++;
        }

        // Resetting the jump counter when player hits the ground/enemy
        if (player_jump_counter < 0 || player_jump_counter >= player_max_jump_counter && bc.IsTouchingLayers())
        {
            player_jump_counter = 0;
        }
    }
}