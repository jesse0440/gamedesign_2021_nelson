using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    // Enemy statistics which are needed elsewhere
    public float enemyHealth = 100f;

    // Enemy statistics only required within this script
    float enemyDamage = 50f;

    // Enemy attack variables
    bool alreadyAttacked = false;
    float attackInterval = 2f;
    float attackTimer;

    void Start() 
    {
        // Assign the current in-game time to the variable
        attackTimer = Time.deltaTime;
    }
    
    void Update() 
    {
        // Check if an interval has passed since this enemy last caused damage
        if (Time.deltaTime > attackTimer + attackInterval)
        {
            // Allow the enemy to cause damage again
            alreadyAttacked = false;
        }
    }
    
    // If enemy is hit from the side or falls on you
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object is a player
        if (collision.gameObject.tag == "Player" && alreadyAttacked == false)
        {
            // Substract the enemy damage from the player's health
            collision.gameObject.GetComponent<PlayerController>().playerHealth -= enemyDamage;

            // Delay the next possible attack with the interval
            alreadyAttacked = true;
            attackTimer = Time.deltaTime;
        }
    }
}