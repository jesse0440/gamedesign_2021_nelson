using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShuriken : MonoBehaviour
{
    // Set relevant variables
    Rigidbody2D rigidBody;
    float projectileDamage = 25f;
    float projectileSpeed = 20f;



    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
    }
    

    // If this projectile collides something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If enemy is hit, damage them
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
