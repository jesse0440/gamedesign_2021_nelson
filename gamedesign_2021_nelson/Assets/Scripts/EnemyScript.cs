using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [Header("Combat Settings")]
    // Enemy variables which are needed elsewhere
    public float enemyHealth;

    // Seconds between possible attacks
    [SerializeField]
    float enemyAttackInterval;

    // Damage enemy does per hit
    [SerializeField]
    float enemyDamage;

    // Bool to determine if this enemy can detect the player
    [SerializeField]
    bool canEnemyDetectPlayer;

    // The range in which the enemy can detect the player
    [SerializeField]
    float playerDetectionRange;

    // Bool to determine if this enemy dies after hitting the player
    [SerializeField]
    bool isSelfdestructing;

    
    
    [Header("Movement Settings")]
    //
    [SerializeField]
    bool canMove;

    // The speed of the enemy
    [SerializeField]
    float enemyMoveSpeed;

    // Bool to determine if this enemy can detect walls
    [SerializeField]
    bool canEnemyDetectWalls;

    // The distance an enemy can cast a ray to check for walls for pathing purposes
    [SerializeField]
    float baseWallCastingDistance;

    // Bool to determine if this enemy can detect edges
    [SerializeField]
    bool canEnemyDetectEdges;

    // The distance an enemy can cast a ray to check for edges for pathing purposes
    [SerializeField]
    float baseEdgeCastingDistance;

    
    
    [Header("Jumping Settings")]
    // Bool to determine if this enemy type can jump
    [SerializeField]
    bool enemyJumpingAllowed;

    // Float to determine this enemy type's jump height
    [SerializeField]
    float enemyJumpHeight;

    // Float to determine this enemy type's interval between jumps
    [SerializeField]
    float enemyJumpInterval;

    // Bool to determine if this enemy type's jump interval should have some RNG
    [SerializeField]
    bool randomJumpIntervalExtender;

    
    
    [Header("Charging Settings")]
    // The speed the enemy uses when charging at a player
    [SerializeField]
    float enemyChargingSpeed;
    
    // Charge at player if can detect player is checked
    [SerializeField]
    bool chargePlayer;

    // Charge at player vertically if can detect player is checked
    [SerializeField]
    bool chargePlayerY;

    
    
    [Header("Drop Settings")]
    // Bool to determine if this enemy can drop heart containers on death
    [SerializeField]
    bool canEnemyDropHearts;

    // The chance of a heart container dropping when this enemy dies
    [SerializeField]
    float enemyDropChance = 0f;

    // Heart container prefab
    [SerializeField]
    GameObject heartContainer;



    [Header("Turret Settings")]
    // Bool to determine if the enemy can shoot
    [SerializeField]
    bool canEnemyShootPlayer;

    // The projectile damage
    [SerializeField]
    public float projectileDamage;

    // The projectile speed
    [SerializeField]
    public float projectileSpeed;

    // Projectile prefab
    [SerializeField]
    GameObject projectile;

    // The point the projectile leaves the enemy
    [SerializeField]
    Transform projectileOutPoint;

    
    
    // Enemy variables only required within this script
    string enemyFacingDirection;

    float enemyMaxHealth;
    float enemyAttackTimer;
    float enemyJumpTimer;
    float randomJumpIntervalExtenderValue;

    bool alreadyJumped = false;
    bool alreadyAttacked = false;
    bool chargeInstanceCheck = false;
    bool wallPatrolCheck = false;
    bool edgePatrolCheck = false;



    // Easy way to make less writing mistakes
    const string LEFT = "left";
    const string RIGHT = "right";

    // Enemy components
    Rigidbody2D rigidBody;
    Transform castingPosition;
    Vector3 baseScale;

    // Transform of the player
    Transform playerTransform;

    
    
    void Start()
    {
        // Check that health is not 0f
        if (enemyHealth == 0f)
        {
            enemyHealth = 25f;
        }

        // Default direction the enemy faces
        enemyFacingDirection = RIGHT;
        // Assign the current time
        enemyAttackTimer = Time.time;
        // Assign the current time
        enemyJumpTimer = Time.time;
        // Assign max health
        enemyMaxHealth = enemyHealth;
        // Assign the rigidbody, the cast position and the base scale of the enemy
        rigidBody = GetComponent<Rigidbody2D>();
        castingPosition = gameObject.transform.Find("CastPosition");
        baseScale = transform.localScale;

        // If jumping is allowed and RNG is turned on for jump intervals
        if (enemyJumpingAllowed && randomJumpIntervalExtender)
        {
            // Increase jump interval with 0-1 seconds
            randomJumpIntervalExtenderValue = Random.value;
        }

        // If jumping is not allowed
        else if (!enemyJumpingAllowed)
        {
            // No need for an RNG interval increase
            randomJumpIntervalExtenderValue = 0f;
        }

        // If the enemy has edge detection
        if (canEnemyDetectEdges)
        {
            // Assign this variable for later
            edgePatrolCheck = true;
        }

        // Otherwise do not assign
        else
        {
            edgePatrolCheck = false;
        }

        // If the enemy has wall detection
        if (canEnemyDetectWalls)
        {
            // Assign this variable for later
            wallPatrolCheck = true;
        }

        // Otherwise do not assign
        else
        {
            wallPatrolCheck = false;
        }


        // Checks for variables that should not be 0f
        // Add as needed!

        if (canEnemyDropHearts)
        {
            if (enemyDropChance == 0f)
            {
                enemyDropChance = 5f;
            }
        }

        if (enemyAttackInterval == 0f)
        {
            enemyAttackInterval = 2f;
        }

        if (enemyDamage < 0f)
        {
            enemyDamage = 10f;
        }

        if (canMove && enemyMoveSpeed == 0f)
        {
            enemyMoveSpeed = 1.25f;
        }

        if (canEnemyDetectWalls)
        {
            if (baseWallCastingDistance == 0f)
            {
                baseWallCastingDistance = 0.01f;
            }
        }

        if (canEnemyDetectEdges)
        {
            if (baseEdgeCastingDistance == 0f)
            {
                baseEdgeCastingDistance = 0.5f;
            }
        }

        if (enemyJumpingAllowed)
        {
            if (enemyJumpHeight == 0f)
            {
                enemyJumpHeight = 6f;
            }

            if (enemyJumpInterval == 0f)
            {
                enemyJumpInterval = 5f;
            }
        }

        if (canEnemyDetectPlayer)
        {
            if (playerDetectionRange == 0f)
            {
                playerDetectionRange = 2f;
            }

            if (chargePlayer)
            {
                if (enemyChargingSpeed == 0f)
                {
                    enemyChargingSpeed = 2.5f;
                }
            }

            if (canEnemyShootPlayer)
            {
                if (projectileDamage == 0f)
                {
                    projectileDamage = 10f;
                }

                if (projectileSpeed == 0f)
                {
                    projectileSpeed = 2f;
                }

                if (projectile == null)
                {
                    projectile = GameObject.FindWithTag("EnemyProjectile");
                }

                if (projectileOutPoint == null)
                {
                    projectileOutPoint = GameObject.Find("ProjectileOutPoint").transform;
                }
            }
        }
    }

    void Update()
    {
        // If the health bugs out over max health or is reduced to 0 or lower
        // Try to spawn a heart container and destroy this enemy object
        if (enemyHealth <= 0 || enemyHealth > enemyMaxHealth)
        {
            if (canEnemyDropHearts)
            {
                float dropRandomValue = Random.Range(1f, 101f);

                if (dropRandomValue <= enemyDropChance)
                {
                    Instantiate(heartContainer, transform.position, transform.rotation);
                }            
            }

            Destroy(gameObject);
        }

        // Check if an interval has passed since this enemy last caused damage
        if (Time.time > enemyAttackTimer + enemyAttackInterval)
        {
            // Allow the enemy to cause damage again
            alreadyAttacked = false;
        }

        // Check if an interval has passed since this enemy last jumped
        if (enemyJumpingAllowed == true && Time.time > enemyJumpTimer + enemyJumpInterval + randomJumpIntervalExtenderValue)
        {
            // Redo the interval RNG and allow the enemy to jump again
            randomJumpIntervalExtenderValue = Random.value;
            alreadyJumped = false;
        }
    }

    // If enemy is collided with
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object is a player
        if (collision.gameObject.tag == "Player")
        {
            // If the enemy's attack interval time has ran out
            if (alreadyAttacked == false)
            {
                // Substract the enemy damage from the player's health
                collision.gameObject.GetComponent<PlayerController>().playerHealth -= enemyDamage;

                // Reset the player's jump counter (Damage boosting)
                collision.gameObject.GetComponent<PlayerController>().playerJumpCounter = 0;

                // Improve the player's jumping height until they touch the ground again
                collision.gameObject.GetComponent<PlayerController>().playerCurrentJumpHeight *= (1f + (1f / 6f));

                // If this enemy is a self-destructing enemy
                if (isSelfdestructing)
                {
                    // Destroys itself
                    Destroy(gameObject);
                }
                
                // Delay the next possible attack with the interval
                alreadyAttacked = true;
                enemyAttackTimer = Time.time;
            }
        }
    }

    // Function that makes the enemy take damage from a player
    public void TakeDamage(float damage)
    {
        //TODO: play hurt animation

        enemyHealth -= damage;
    }

    // Framerate independent Update(), works better for physics
    private void FixedUpdate()
    {
        // Clone the enemy's move speed
        float newSpeed = enemyMoveSpeed;

        // If facing left, move to the left
        if (enemyFacingDirection == LEFT)
        {
            newSpeed = -enemyMoveSpeed;
        }

        // If facing right, move to the right
        else if (enemyFacingDirection == RIGHT)
        {
            newSpeed = enemyMoveSpeed;
        }

        // Move the enemy based on the assigned speed
        if (canMove)
        {
            rigidBody.velocity = new Vector2(newSpeed, rigidBody.velocity.y);
        }

        // Determine if the enemy is hitting a wall
        if (canEnemyDetectWalls && IsHittingWall() || canEnemyDetectEdges && IsNearEdge() && rigidBody.velocity.y == 0)
        {
            // If going left before hitting a wall/nearing an edge
            if (enemyFacingDirection == LEFT)
            {
                // Change direction to right
                ChangeFacingDirection(RIGHT);
            }

            // If going right before hitting a wall/nearing an edge
            else if (enemyFacingDirection == RIGHT)
            {
                // Change direction to left
                ChangeFacingDirection(LEFT);
            }
        }

        // If jumping is allowed and enemy has not jumped in an interval
        if (enemyJumpingAllowed == true && alreadyJumped == false)
        {
            // Jump using the enemy's jump height, reset the jump timer and make the enemy unable to jump for the next interval
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, enemyJumpHeight);
            alreadyJumped = true;
            enemyJumpTimer = Time.time;
        }

        // If player detection is turned on
        if (canEnemyDetectPlayer)
        {
            // Find the player's transform and the distance between the enemy and the player
            playerTransform = GameObject.FindWithTag("Player").transform;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // If charging is enabled
            if (chargePlayer)
            {
                // If the player is inside the detection range
                if (distanceToPlayer <= playerDetectionRange)
                {
                    //  If either of these detections is true, disable both (saves time & space)
                    if (wallPatrolCheck || edgePatrolCheck)
                    {
                        // Disable both patrols
                        canEnemyDetectEdges = false;
                        canEnemyDetectWalls = false;
                    }

                    // This charge instance at player has started
                    chargeInstanceCheck = true;

                    // Charge at player
                    ChargeAtPlayer();
                }

                // If the player is outside the detection range
                else if (distanceToPlayer > playerDetectionRange && chargeInstanceCheck == true)
                {
                    // This charge instance at player has ended
                    chargeInstanceCheck = false;

                    // Leave the player alone
                    StopChargingAtPlayer();
                }
            }

            // If shooting is enabled
            if (canEnemyShootPlayer)
            {
                // If the player is inside the detection range
                if (distanceToPlayer <= playerDetectionRange)
                {
                    // If the enemy has not attacked in the attack interval
                    if (!alreadyAttacked)
                    {
                        // Shoot at the player
                        ShootAtPlayer();
                    }
                }
            }
        }
    }

    // Change the direction the enemy is moving in
    private void ChangeFacingDirection(string newDirection)
    {
        // Clone the base scale of the enemy
        Vector3 newScale = baseScale;

        // If the enemy is moving left
        if (newDirection == LEFT)
        {
            // Change the scale from 1 to -1 (Flips the sprite)
            newScale.x = -baseScale.x;
        }

        // If the enemy is moving right
        else if (newDirection == RIGHT)
        {
            // Change the scale from -1 to 1 (Flips the sprite)
            newScale.x = baseScale.x;
        }

        // Assign the new scale as this enemy object's scale
        transform.localScale = newScale;

        // Assign the new direction as this enemy's facing direction
        enemyFacingDirection = newDirection;
    }

    // Determine if the enemy is hitting a wall and should change direction
    bool IsHittingWall()
    {
        // Local variables
        bool value = false;
        float castingDistance = baseWallCastingDistance;

        // If the enemy is looking to the left, make the cast distance negative
        if (enemyFacingDirection == LEFT)
        {
            castingDistance = -baseWallCastingDistance;
        }

        // If the enemy is looking to the right, make the cast distance positive
        else if (enemyFacingDirection == RIGHT)
        {
            castingDistance = baseWallCastingDistance;
        }

        // The target the enemy is trying to reach
        Vector3 enemyTargetPosition = castingPosition.position;
        enemyTargetPosition.x += castingDistance;

        // A line to be drawn for debugging purposes
        // Debug.DrawLine(castingPosition.position, enemyTargetPosition, Color.red);

        // Determine if the cast position casting a ray towards the enemy's target position hits anything within the layer "Terrain"
        if (Physics2D.Linecast(castingPosition.position, enemyTargetPosition, 1 << LayerMask.NameToLayer("Terrain")))
        {
            // Hitting a wall
            value = true;
        }

        else
        {
            // Not hitting a wall
            value = false;
        }

        // Return whether the enemy is hitting a wall or not
        return value;
    }

    // Determine if the enemy is near an edge and should change direction
    bool IsNearEdge()
    {
        // Local variables
        bool value = true;
        float castingDistance = baseEdgeCastingDistance;

        // The edge the enemy is trying to avoid
        Vector3 enemyTargetPosition = castingPosition.position;
        enemyTargetPosition.y -= castingDistance;

        // A line to be drawn for debugging purposes
        // Debug.DrawLine(castingPosition.position, enemyTargetPosition, Color.blue);

        // Determine if the cast position casting a ray towards the enemy's target position hits anything within the layer "Terrain"
        if (Physics2D.Linecast(castingPosition.position, enemyTargetPosition, 1 << LayerMask.NameToLayer("Terrain")))
        {
            // Not near an edge
            value = false;
        }

        else
        {
            // Near an edge
            value = true;
        }

        // Return whether the enemy is near an edge or not
        return value;
    }

    // Charge at the player when called
    private void ChargeAtPlayer()
    {
        // Clone the rigidbody's velocity
        Vector2 newVelocity = rigidBody.velocity;

        // If the player is exactly at the same x coordinate
        if (transform.position.x + 0.1f >= playerTransform.position.x && transform.position.x - 0.1f <= playerTransform.position.x)
        {
            // Stop moving, look right
            newVelocity.x = 0;
            transform.localScale = new Vector2(1, 1);
        }

        // If the player is to the right of the enemy
        else if (transform.position.x + 0.1f < playerTransform.position.x)
        {
            // Move right, look right
            newVelocity.x = enemyChargingSpeed;
            transform.localScale = new Vector2(1, 1);
        }

        // If the player is to the left of the enemy
        else if (transform.position.x - 0.1f > playerTransform.position.x)
        {
            // Move left, look left
            newVelocity.x = -enemyChargingSpeed;
            transform.localScale = new Vector2(-1, 1);
        }

        // Charging vertically
        if (chargePlayerY)
        {
            if (transform.position.y + 0.1f >= playerTransform.position.y && transform.position.y - 0.1f <= playerTransform.position.y)
            {
                // Stop going up and down
                newVelocity.y = 0;
            }

            // If the player is above the enemy
            else if (transform.position.y + 0.1f < playerTransform.position.y)
            {
                // Move up
                newVelocity.y = enemyChargingSpeed;
            }

            // If the player is below the enemy
            else if (transform.position.y - 0.1f > playerTransform.position.y)
            {
                // Move down
                newVelocity.y = -enemyChargingSpeed;
            }
        }

        // Assign the rigidbody's new velocity
        rigidBody.velocity = newVelocity;
    }

    // Stop charging at player when called
    private void StopChargingAtPlayer()
    {
        // Stop the enemy chase and resume patrol
        if (chargePlayerY)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }

        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        // If the enemy has wall and edge patrol
        if (wallPatrolCheck && edgePatrolCheck)
        {
            // Enable both modes and start patrol
            canEnemyDetectWalls = true;
            canEnemyDetectEdges = true;
            ChangeFacingDirection(RIGHT);
        }

        // If the enemy has wall patrol only
        else if (wallPatrolCheck)
        {
            // Enable wall mode and start patrol
            canEnemyDetectWalls = true;
            ChangeFacingDirection(RIGHT);
        }

        // If the enemy has edge patrol only
        else if (edgePatrolCheck)
        {
            // Enable edge mode and start patrol
            canEnemyDetectEdges = true;
            ChangeFacingDirection(RIGHT);
        }
    }

    // Shooting function
    private void ShootAtPlayer()
    {
        // Instantiate a projectile and set its damage and speed
        GameObject shotProjectile = (GameObject)Instantiate(projectile, projectileOutPoint.position, projectileOutPoint.rotation);
        EnemyProjectile enemyProjectile = shotProjectile.GetComponent<EnemyProjectile>();
        enemyProjectile.SetVariables(projectileDamage, projectileSpeed);

        // Delay the next possible attack with the interval
        alreadyAttacked = true;
        enemyAttackTimer = Time.time;
    }
}