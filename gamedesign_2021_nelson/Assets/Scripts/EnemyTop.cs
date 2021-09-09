using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTop : MonoBehaviour
{
    // The enemy's parent object
    public GameObject enemy_parent_object;

    // Assign the enemy parent object
    void Start()
    {
        enemy_parent_object = transform.parent.gameObject;
    }

    // When the player jumps on the top of this enemy
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // Destroy the enemy parent object
        Destroy(enemy_parent_object);
    }
}