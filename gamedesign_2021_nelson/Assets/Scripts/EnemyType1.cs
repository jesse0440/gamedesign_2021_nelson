using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    // Enemy variables which are needed elsewhere
    public float enemyHealth;

    // Seconds between possible attacks
    [SerializeField]
    float enemyAttackInterval;

    // Damage enemy does per hit
    [SerializeField]
    float enemyDamage;

    // The speed of the enemy
    [SerializeField]
    float enemyMoveSpeed;

    // The distance an enemy can cast a ray to check for walls for pathing purposes
    [SerializeField]
    float baseWallCastingDistance;

    // The distance an enemy can cast a ray to check for edges for pathing purposes
    [SerializeField]
    float baseEdgeCastingDistance;

    // Enemy variables only required within this script
    bool alreadyAttacked = false;
    float enemyAttackTimer;
    float enemyMaxHealth;
    string enemyFacingDirection;

    // Easy way to make less writing mistakes
    const string LEFT = "left";
    const string RIGHT = "right";

    // Enemy components
    Rigidbody2D rigidBody;
    Transform castingPosition;
    Vector3 baseScale;

    void Start() 
    {
        // Default direction the enemy faces
        enemyFacingDirection = RIGHT;
        // Assign the current time
        enemyAttackTimer = Time.time;
        // Assign max health
        enemyMaxHealth = enemyHealth;
        // Assign the rigidbody, the cast position and the base scale of the enemy
        rigidBody = GetComponent<Rigidbody2D>();
        castingPosition = gameObject.transform.Find("CastPosition");
        baseScale = transform.localScale;
    }
    
    void Update() 
    {
        // If the health bugs out over max health or is reduced to 0 or lower, destroy this enemy object
        if (enemyHealth <= 0 || enemyHealth > enemyMaxHealth)
        {
            Destroy(gameObject);
        }

        // Check if an interval has passed since this enemy last caused damage
        if (Time.time > enemyAttackTimer + enemyAttackInterval)
        {
            // Allow the enemy to cause damage again
            alreadyAttacked = false;
        }
    }
    
    // If enemy is hit from the side or falls on you
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

                // Delay the next possible attack with the interval
                alreadyAttacked = true;
                enemyAttackTimer = Time.time;
            }
        }

        // Not in use yet
        // If an enemy is hit by a weapon, substract that damage from health
        /*
        if (collision.gameObject.tag == "Weapon")
        {
            enemyHealth -= collision.gameObject.GetComponent<WeaponScript>().weaponDamage;
        }
        */
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
        rigidBody.velocity = new Vector2(newSpeed, rigidBody.velocity.y);

        // Determine if the enemy is hitting a wall or located near an edge
        if (IsHittingWall() || IsNearEdge())
        {
            // If going left before hitting a wall
            if (enemyFacingDirection == LEFT)
            {
                // Change direction to right
                ChangeFacingDirection(RIGHT);
            }

            // If going right before hitting a wall
            else if (enemyFacingDirection == RIGHT)
            {
                // Change direction to left
                ChangeFacingDirection(LEFT);
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

        else if (enemyFacingDirection == RIGHT)
        {
            castingDistance = baseWallCastingDistance;
        }

        // The target the enemy is trying to reach
        Vector3 enemyTargetPosition = castingPosition.position;
        enemyTargetPosition.x += castingDistance;

        // A line to be drawn for debugging purposes
        Debug.DrawLine(castingPosition.position, enemyTargetPosition, Color.red);

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
        Debug.DrawLine(castingPosition.position, enemyTargetPosition, Color.blue);

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
}