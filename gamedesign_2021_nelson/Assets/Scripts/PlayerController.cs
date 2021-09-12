using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    Slider playerHealthBarSlider;

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


        // Assign the health bar HUD element to the variable
        playerHealthBarSlider = GameObject.FindWithTag("PlayerHealthBar").GetComponent<Slider>();
        playerHealthBarSlider.maxValue = playerMaxHealth;

        // Import the coordinates to your location in the room or use default if unavailable, then warp to the location
        float tempXCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " X Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.x);
        float tempYCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " Y Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.y);
        transform.position = new Vector2(tempXCoordinate, tempYCoordinate);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player's health drops to zero or bugs out to negative
        if (playerHealth <= 0) 
        {
            // Respawn the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // If an health container would overheal the player or the player's health bugs out to over max
        else if (playerHealth > playerMaxHealth)
        {
            // Reduce the player's health to the maximum
            playerHealth = playerMaxHealth;
        }

        // Update the health bar in the HUD if player health changes
        playerHealthBarSlider.value = playerHealth;
        
        // Get the Horizontal input of Input manager
        float horizontalDirection = Input.GetAxis("Horizontal");

        // The player movement
        rigidBody.velocity = new Vector2(horizontalDirection * playerSpeed, rigidBody.velocity.y);

        // While moving right
        if (horizontalDirection > 0) 
        {
            // Make the local scale's X positive to make the player face right
            Vector3 newScale = new Vector3(1, 1, 1);
            transform.localScale = newScale;
        }

        // While moving left
        if (horizontalDirection < 0)
        {
            // Make the local scale's X negative to make the player face left
            Vector3 newScale = new Vector3(-1, 1, 1);
            transform.localScale = newScale;
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