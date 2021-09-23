using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShuriken : MonoBehaviour
{
    // Set relevant variables
    Rigidbody2D rigidBody;

    [SerializeField]
    float projectileDamage;
    [SerializeField]
    float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        if (projectileDamage == 0f)
        {
            projectileDamage = 15f;
        }

        if (projectileSpeed == 0f)
        {
            projectileSpeed = 10f;
        }
        
        rigidBody.velocity = transform.right * projectileSpeed;
    }
    
    // If this projectile collides something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If enemy is hit damage them and destroy the shuriken
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

        // If terrain is hit destroy the shuriken
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}