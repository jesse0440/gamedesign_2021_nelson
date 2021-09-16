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
    public float dashUnlockedCheck = 0f;
    public float dashDistance = 4f;
    public float dashInterval = 2f;

    //Combat variables and components
    [SerializeField]
    float attackRange = 0.5f;
    [SerializeField]
    float meleeDamage = 10f;
    [SerializeField]
    float meleeAttackInterval = 2f;

    // Player statistics which are only needed in this script
    float playerMaxHealth = 100f;
    float playerSpeed = 7f;
    float playerBaseJumpHeight = 12f;
    float groundedCheckRayLength = 0.01f;
    float dashIntervalTimer;
    float nextMeleeTimer;
    float groundedIntervalTimer;
    bool groundedIntervalPassed;
    bool meleeIntervalPassed;
    bool dashIntervalPassed;
    bool dashUsed;
    
    // Player components
    Animator playerAnimator;
    Rigidbody2D rigidBody;
    EdgeCollider2D edgeCollider;
    SpriteRenderer spriteRenderer;
    Transform attackPoint;
    LayerMask terrainLayerMask;
    LayerMask enemyLayers;
    Slider playerHealthBarSlider;
    Color rayColor;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the player's current jump height as the base jump height
        playerCurrentJumpHeight = playerBaseJumpHeight;

        // Find the necessary components of the player object
        playerAnimator = GetComponent<Animator>();
        attackPoint = GameObject.Find("attackPoint").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        terrainLayerMask = LayerMask.GetMask("Terrain");
        enemyLayers = LayerMask.GetMask("Enemies");

        // Find if there is a saved amount of health for the player or use default (100f)
        playerHealth = PlayerPrefs.GetFloat("PlayerHealth", 100f);

        // Set the interval comparison time for dashing
        dashIntervalTimer = Time.time;

        // Set the interval comparison time for melee attacks
        nextMeleeTimer = Time.time;

        // Setup your grounded timer comparison
        groundedIntervalTimer = Time.time;


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
        playerMaxJumpCounter = PlayerPrefs.GetFloat("Ability_0", 1);

        // ABILITY 1 - Find out if wall climb is unlocked (0.02f) or use default value (0)
        wallClimbValue = PlayerPrefs.GetFloat("Ability_1", 0);

        // ABILITY 2 - Find out if dash is unlocked (1) or use default value (0)
        dashUnlockedCheck = PlayerPrefs.GetFloat("Ability_2", 0);
        
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

        // Jumping up with W or Up Arrow if your jump counter is not maxed
        if (Input.GetButtonDown("Jump") && playerJumpCounter < playerMaxJumpCounter)
        {
            Jump();
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetButtonDown("Jump2") && playerJumpCounter < playerMaxJumpCounter)
        {
            Jump();
        }

        // If your grounded interval has passed
        if (Time.time > groundedIntervalTimer + 0.1f)
        {
            // Resetting jumps is once again allowed
            groundedIntervalPassed = true;
        }

        // Check if enough time has passed since last melee attack
        if (Time.time > nextMeleeTimer + meleeAttackInterval)
        {
            meleeIntervalPassed = true;
        }
        
        // Check if enough time has passed since last use of Dash
        if (dashUnlockedCheck == 1 && Time.time > dashIntervalTimer + dashInterval)
        {
            dashIntervalPassed = true;
        }
        
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

        // Dashing with Left Shift if it is unlocked
        if (Input.GetButtonDown("Dash") && dashUnlockedCheck == 1 && dashIntervalPassed)
        {
            // Mark that Dash was used
            dashUsed = true;

            // Reset Dash timer
            dashIntervalPassed = false;
            dashIntervalTimer = Time.time;
        }

        // Check for melee attack input
        if (Input.GetButtonDown("Attack") && meleeIntervalPassed)
        {
            // Attack
            MeleeAttack();

            // Reset attack timer
            meleeIntervalPassed = false;
            nextMeleeTimer = Time.time;
        }

        // Resetting the jump counter & jump height damage boost when player hits the ground
        // Resetting the timer until you can next reset your jumps
        if (IsGrounded() && groundedIntervalPassed)
        {
            playerCurrentJumpHeight = playerBaseJumpHeight;
            playerJumpCounter = 0;

            groundedIntervalPassed = false;
            groundedIntervalTimer = Time.time;
        }
    }

    // Fixed update occurs at the same time regardless of framerate
    private void FixedUpdate()
    {
        // If the dash was used
        if (dashUsed)
        {
            // Move the player for dash distance and set the ability on cooldown
            rigidBody.MovePosition(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0) * dashDistance);
            dashUsed = false;
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
        return rayCastHit.collider;
    }  

    private void MeleeAttack()
    {
        playerAnimator.SetTrigger("useMelee");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(meleeDamage);
        }
    }

    private void OnDrawGizmosSelected() {

        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Jump() 
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerCurrentJumpHeight);
        playerJumpCounter += 1;
    }
}