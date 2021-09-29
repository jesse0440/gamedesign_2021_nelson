using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Every customizable value
    [SerializeField]
    float platformSpeed;
    [SerializeField]
    bool hasVerticalDetection;
    [SerializeField]
    float verticalDetectionRange;
    
    // Easy way to make less writing mistakes
    const string UP = "up";
    const string DOWN = "down";

    // Script variables
    Rigidbody2D rigidBody;
    Transform castingPosition;
    string platformDirection;
    float castingPositionOffset = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        // Set default direction, assign variables
        platformDirection = UP;
        rigidBody = GetComponent<Rigidbody2D>();
        castingPosition = gameObject.transform.Find("CastingPosition");

        // Make sure some important values are not 0f
        if (platformSpeed == 0f)
        {
            platformSpeed = 2f;
        }

        if (hasVerticalDetection && verticalDetectionRange == 0f)
        {
            verticalDetectionRange = 3f;
        }
    }

    // Framerate independent Update(), works better for physics
    void FixedUpdate()
    {
        // Clone speed
        float newSpeed = platformSpeed;

        // Check direction and change speed
        if (platformDirection == UP)
        {
            newSpeed = platformSpeed;
        }

        else if (platformDirection == DOWN)
        {
            newSpeed = -platformSpeed;
        }

        // Move the platform
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, newSpeed);

        // If near a wall
        if (hasVerticalDetection && IsHittingVerticalWall())
        {
            // Change direction
            if (platformDirection == UP)
            {
                platformDirection = DOWN;
            }

            else if (platformDirection == DOWN)
            {
                platformDirection = UP;
            }
        }
    }

    // Determine if the enemy is hitting a vertical wall and should change direction
    bool IsHittingVerticalWall()
    {
        // Local variables
        bool value = false;
        float castingDistance = verticalDetectionRange;
        float verticalOffset = castingPositionOffset;

        // If the platform is going up
        if (platformDirection == UP)
        {
            // Set the variables for the detection vector to positive
            castingDistance = verticalDetectionRange;
            verticalOffset = castingPositionOffset;
        }

        // If the platform is going down
        else if (platformDirection == DOWN)
        {
            // Set the variables for the detection vector to negative
            castingDistance = -verticalDetectionRange;
            verticalOffset = -castingPositionOffset;
        }

        // The target the enemy is trying to reach
        Vector3 platformTargetPosition = castingPosition.position;
        platformTargetPosition.y += castingDistance;

        // Vector with offset to prevent the platform from detecting itself
        Vector2 offsetVector = new Vector3(castingPosition.position.x, castingPosition.position.y + verticalOffset, castingPosition.position.z);

        // A line to be drawn for debugging purposes
        Debug.DrawLine(offsetVector, platformTargetPosition, Color.red);

        // Determine if the cast position casting a ray upwards hits anything within the layer "Terrain"
        if (Physics2D.Linecast(offsetVector, platformTargetPosition, 1 << LayerMask.NameToLayer("Terrain")))
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
}