using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Set relevant variables
    Rigidbody2D rigidBody;
    Transform projectileTarget;
    float projectileDamage;
    float projectileSpeed;
    bool hasBeenShot = false;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Rigidbody2D and player
        rigidBody = GetComponent<Rigidbody2D>();
        projectileTarget = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If this projectile has been shot
        if (hasBeenShot)
        {
            // Assign direction, start moving the bullet, wait for next shot
            Vector3 projectileDirection = (projectileTarget.position - transform.position).normalized;
            rigidBody.velocity = projectileDirection * projectileSpeed;
            hasBeenShot = false;
        }
    }

    // If this projectile collides something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If it is a player do damage to them and self-destruct
        if (collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().playerHealth -= projectileDamage;
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    // Set the damage and speed of the projectile
    public void SetVariables(float tempProjectileDamage, float tempProjectileSpeed)
    {
        projectileDamage = tempProjectileDamage;
        projectileSpeed = tempProjectileSpeed;
        hasBeenShot = true;
    }
}