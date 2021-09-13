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
    public float playerCurrentJumpHeight;
    public float wallClimbValue = 0f;

    // Player statistics which are only needed in this script
    float playerMaxHealth = 100f;
    float playerSpeed = 7f;
    float playerBaseJumpHeight = 12f;
    float groundedCheckRayLength = 0.15f;
    
    // Player components
    Rigidbody2D rigidBody;
    EdgeCollider2D edgeCollider;
    SpriteRenderer spriteRenderer;
    LayerMask terrainLayerMask;
    Slider playerHealthBarSlider;
    Color rayColor;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the player's current jump height as the base jump height
        playerCurrentJumpHeight = playerBaseJumpHeight;

        // Find the necessary components of the player object
        rigidBody = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        terrainLayerMask = LayerMask.GetMask("Terrain");

        // Find if there is a saved amount of health for the player or use default (100f)
        playerHealth = PlayerPrefs.GetFloat("PlayerHealth", 100f);


        /*
         ___________________________________________________________________
        |                       ABILITY TEMPLATE                            |
        |                                                                   |
        |           variable = PlayerPrefs.GetInt("Ability_X", 0);          |
        |                                                                   |
        |___________________________________________________________________|
        */

        // ----------ABILITIES----------

        // ABILITY 0 - Find out if double jump is unlocked (2) or use default value (1)
        playerMaxJumpCounter = PlayerPrefs.GetInt("Ability_0", 1);

        // ABILITY 1 - Find out if wall climb is unlocked (0.02f) or use default value (0)
        wallClimbValue = PlayerPrefs.GetInt("Ability_1", 0);

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
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerCurrentJumpHeight);
            playerJumpCounter++;
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetButtonDown("Jump2") && playerJumpCounter < playerMaxJumpCounter)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerCurrentJumpHeight);
            playerJumpCounter++;
        }

        // Check if the player is grounded
        IsGrounded();

        // Resetting the jump counter & jump height damage boost when player hits the ground
        if (rigidBody.velocity.y == 0 && IsGrounded())
        {
            playerJumpCounter = 0;
            playerCurrentJumpHeight = playerBaseJumpHeight;
        }
    }

    // The function which checks if the player is grounded
    private bool IsGrounded() 
    {
        // Assign the vector with possible ability and cast the ray collider
        Vector3 wallClimbVector = new Vector3(wallClimbValue, 0, 0);
        RaycastHit2D rayCastHit = Physics2D.BoxCast(edgeCollider.bounds.center, edgeCollider.bounds.size + wallClimbVector, 0f, Vector2.down, groundedCheckRayLength, terrainLayerMask);

        // This code block is only for debugging purposes
        // It allows you to see the area of the ray in color as you jump up and down
        
        /*
        // If grounded
        if (rayCastHit.collider != null)
        {
            // Green gizmo color
            rayColor = Color.green;
        }

        // If not grounded
        else
        {
            // Red gizmo color
            rayColor = Color.red;
        }

        // Visualize the ray collider area
        Debug.DrawRay(edgeCollider.bounds.center + new Vector3(edgeCollider.bounds.extents.x, 0), Vector2.down * (edgeCollider.bounds.extents.y + groundedCheckRayLength), rayColor);
        Debug.DrawRay(edgeCollider.bounds.center - new Vector3(edgeCollider.bounds.extents.x, 0), Vector2.down * (edgeCollider.bounds.extents.y + groundedCheckRayLength), rayColor);
        Debug.DrawRay(edgeCollider.bounds.center - new Vector3(edgeCollider.bounds.extents.x, edgeCollider.bounds.extents.y + groundedCheckRayLength), 2f * Vector2.right * (edgeCollider.bounds.extents.x), rayColor);
        
        // Use this to check for hit timings if needed
        // Debug.Log(rayCastHit.collider);
        */

        // Return the value so script knows whether the player's jump counter is reset or not
        return rayCastHit.collider != null;
    }
}