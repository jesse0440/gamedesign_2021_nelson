using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTop : MonoBehaviour
{
    // The enemy's parent object
    GameObject enemyParentObject;

    // Assign the enemy parent object
    void Start()
    {
        enemyParentObject = transform.parent.gameObject;
    }

    // When the player jumps on the top of this enemy
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // If the collided object is a player
        if (collision.gameObject.tag == "Player")
        {
            // Destroy the enemy parent object
            Destroy(enemyParentObject);
        }
    }
}