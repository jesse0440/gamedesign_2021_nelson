using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    // Enemy statistics
    public float enemy_health = 100;
    public float enemy_damage = 50;
    
    // If enemy is hit from the side or falls on you
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object is a player
        if (collision.gameObject.tag == "Player")
        {
            // Substract the enemy damage from the player's health
            collision.gameObject.GetComponent<PlayerController>().player_health -= enemy_damage;
        }
    }
}