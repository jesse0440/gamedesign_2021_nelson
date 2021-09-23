using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Public variables in categories
    [Header("Combat Settings")]
    public float playerHealth = 100f;
    [HideInInspector]
    public float playerMaxHealth = 100f;
    [SerializeField]
    float attackRange = 0.5f;
    [SerializeField]
    float meleeDamage = 10f;
    [SerializeField]
    float meleeAttackInterval = 2f;

    [Header("Jump Settings")]
    public float playerMaxJumpCounter = 1;
    public float playerJumpCounter = 0;
    [SerializeField]
    float playerBaseJumpHeight = 12f;
    public float playerCurrentJumpHeight;
    [SerializeField]
    float playerGravity = 2.5f;
    [SerializeField]
    float fallingGravityMultiplier = 2f;

    [Header("Ability Settings")]
    public float wallClimbValue = 0f;
    public float dashUnlockedCheck = 0f;
    public float dashDistance = 4f;
    public float dashInterval = 2f;

    

    // Player statistics which are only needed in this script
    float playerSpeed = 10f;
    float playerMaxSpeed = 8f;
    float groundedCheckRayLength = 0.01f;
    float dashIntervalTimer;
    float nextMeleeTimer;
    bool meleeIntervalPassed;
    bool dashIntervalPassed;
    bool hasNotJumped = true;
    bool dashUsed;
    

    
    // Player components
    Animator playerAnimator;
    Rigidbody2D rigidBody;
    EdgeCollider2D edgeCollider;
    SpriteRenderer spriteRenderer;
    Vector2 playerDirection;
    Transform attackPoint;
    LayerMask terrainLayerMask;
    LayerMask enemyLayers;
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

        // Set gravity to a default if not set in editor
        if (playerGravity == 0)
        {
            playerGravity = 2.5f;
        }


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
        
        // ABILITY 3 - Find out if "Omae Wa Mou Shindeiru" is unlocked (x) or use default value (y)
        // abilityThreeCheck = PlayerPrefs.GetFloat("Ability_3", y);

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

        // Jumping up with W or Up Arrow if your jump counter is not maxed
        if (Input.GetButtonDown("Jump") && playerJumpCounter < playerMaxJumpCounter)
        {
            hasNotJumped = false;
            Jump();
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetButtonDown("Jump2") && playerJumpCounter < playerMaxJumpCounter)
        {
            hasNotJumped = false;
            Jump();
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
        playerDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // While moving right
        if (playerDirection.x > 0) 
        {
            // Make the local scale's X positive to make the player face right
            Vector3 newScale = new Vector3(1, 1, 1);
            transform.localScale = newScale;
        }

        // While moving left
        if (playerDirection.x < 0)
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
    }

    // Fixed update occurs at the same time regardless of framerate
    private void FixedUpdate()
    {
        // Resetting the jump counter & jump height damage boost when player hits the ground
        // Inform the system that the player can fall again
        // Set gravity back to normal
        if (IsGrounded() && rigidBody.velocity.y == 0 || IsWallClimbing())
        {
            playerCurrentJumpHeight = playerBaseJumpHeight;
            playerJumpCounter = 0;
            hasNotJumped = true;
            rigidBody.gravityScale = playerGravity;
        }

        // Call movement
        PlayerMovement(playerDirection.x);

        // If falling and vertical velocity is lower than the bump velocity limit
        if (rigidBody.velocity.y < -1.67f)
        {
            // Increase gravity by the multiplier
            rigidBody.gravityScale = playerGravity * fallingGravityMultiplier;
        }
        
        // If the dash was used
        if (dashUsed)
        {
            // Move the player for dash distance and set the ability on cooldown
            rigidBody.MovePosition(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0) * dashDistance);
            dashUsed = false;
        }

        // If the player has not jumped but their vertical velocity is lower than the bump velocity limit
        if (hasNotJumped && rigidBody.velocity.y < -1.67f)
        {
            // Count it as a fall and increase jump counter
            playerJumpCounter += 1;
            hasNotJumped = false;
        }
    }

    // The player movement function
    private void PlayerMovement(float direction)
    {
        // Set the speed and direction
        rigidBody.velocity = new Vector2(direction * playerSpeed, rigidBody.velocity.y);

        // If velocity exceeds the player's max speed
        if (Mathf.Abs(rigidBody.velocity.x) > playerMaxSpeed)
        {
            // Limit velocity
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * playerMaxSpeed, rigidBody.velocity.y);
        }
    }

    // The function which checks if the player is grounded
    private bool IsGrounded() 
    {
        // Cast the box to check for ground
        RaycastHit2D rayCastHit = Physics2D.BoxCast(edgeCollider.bounds.center, new Vector3(edgeCollider.bounds.size.x / 2, edgeCollider.bounds.size.y, edgeCollider.bounds.size.z), 0f, Vector2.down, groundedCheckRayLength, terrainLayerMask);

        // Return the value so script knows whether the player's jump counter is reset or not
        return rayCastHit.collider;
    }

    // The function which checks if the player is wall climbing
    private bool IsWallClimbing()
    {
        // Cast the box to check for walls
        RaycastHit2D rayCastHit = Physics2D.BoxCast(edgeCollider.bounds.center, new Vector3(edgeCollider.bounds.size.x + wallClimbValue, edgeCollider.bounds.size.y / 2, edgeCollider.bounds.size.z), 0f, Vector2.down, groundedCheckRayLength, terrainLayerMask);

        // Return the value so script knows whether the player's jump counter is reset or not
        return rayCastHit.collider;
    }

    // Melee attack function
    private void MeleeAttack()
    {
        // Create attack collider
        playerAnimator.SetTrigger("useMelee");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // If enemy colliders are inside the collider
        foreach(Collider2D enemy in hitEnemies)
        {
            // Damage enemies
            enemy.GetComponent<EnemyScript>().TakeDamage(meleeDamage);
        }
    }

    // Draw the attack area while in editor
    private void OnDrawGizmosSelected() {

        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Jump function
    private void Jump() 
    {
        // Set gravity and drag to normal, add an upwards force to the player and increase jump counter
        rigidBody.gravityScale = playerGravity;
        rigidBody.drag = 0f;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * playerCurrentJumpHeight, ForceMode2D.Impulse);
        playerJumpCounter += 1;
    }

    
    //save function
    public void SavePlayer()
    {
        //saves the player
        SaveSystem.SavePlayer(this);
        // just shows where it saves
        Debug.Log(Application.persistentDataPath);
    }

    //loads player
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        //sets player position
        Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];
        transform.position = position;
    }
    
}