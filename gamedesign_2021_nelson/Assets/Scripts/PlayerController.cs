using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Player statistics which are accessed from elsewhere
    public float playerHealth = 100f;
    public float playerMaxJumpCounter = 1;
    public float playerJumpCounter = 0;

    // Player statistics which are only needed in this script
    float playerMaxHealth = 100f;
    float playerSpeed = 7f;
    float playerJumpHeight = 12f;
    
    // Player components
    Rigidbody2D rigidBody;
    PolygonCollider2D polygonCollider;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Find the necessary components of the player object
        rigidBody = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        /*
         ___________________________________________________________________
        |                       ABILITY TEMPLATE                            |
        |                                                                   |
        |           variable = PlayerPrefs.GetInt("Ability_X", 0);          |
        |                                                                   |
        |___________________________________________________________________|
        */

        // ----------ABILITIES----------

        // ABILITY 0 - Find out if double jump is unlocked or use default value (1)
        playerMaxJumpCounter = PlayerPrefs.GetInt("Ability_0", 1);

        // ABILITY 1 - 
        // code

        // ABILITY 2 - 
        // code
        
        // ABILITY 3 - 
        // code


        // Import the coordinates to your location in the room or use default if unavailable, then warp to the location
        float tempXCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " X Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.x);
        float tempYCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " Y Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.y);
        transform.position = new Vector2(tempXCoordinate, tempYCoordinate);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player's health drops to zero or bugs out otherwise
        if (playerHealth > playerMaxHealth || playerHealth <= 0) 
        {
            // Respawn the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Get the Horizontal input of Input manager
        float horizontalDirection = Input.GetAxis("Horizontal");

        // The player movement
        rigidBody.velocity = new Vector2(horizontalDirection * playerSpeed, rigidBody.velocity.y);

        // While moving right
        if (horizontalDirection > 0) 
        {
            // Flip sprite to face right
            spriteRenderer.flipX = false;
        }

        // While moving left
        if (horizontalDirection < 0)
        {
            // Flip sprite to face left
            spriteRenderer.flipX = true;
        }

        // Jumping up with W or Up Arrow if your jump counter is not maxed
        if (Input.GetButtonDown("Jump") && playerJumpCounter < playerMaxJumpCounter)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerJumpHeight);
            playerJumpCounter++;
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetButtonDown("Jump2") && playerJumpCounter < playerMaxJumpCounter)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerJumpHeight);
            playerJumpCounter++;
        }

        // Resetting the jump counter when player hits the ground/enemy/wall
        if (rigidBody.velocity.y == 0 && polygonCollider.IsTouchingLayers())
        {
            playerJumpCounter = 0;
        }
    }
}