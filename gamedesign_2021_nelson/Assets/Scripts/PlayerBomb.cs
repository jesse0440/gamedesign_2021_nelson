using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    // Sprite renderer and explosion sprite
    // TODO: play explosion animation
    public SpriteRenderer spriteRenderer;
    public Sprite explosionSprite;

    // Speed, explosion timer and impact damage of the projectile
    public float speed = 25;
    public float impactDamage = 10f;
    public float explodeTime = 2f;
    
    // Explosion radius and damage
    public float splashRange = 1f;
    public float explosionDamage = 50f;
    bool hasImpacted = false;

    // Vertical movement vectors of the bomb
    public Vector3 xForce;
    public Vector3 yForce;

    // Start is called before the first frame update
    void Start()
    {
        // Multiply xForce with transform.right to keep the proper rotation
        Vector3 xForceScaled = Vector3.Scale(transform.right, xForce);
        // Combine x- and y- vectors to get the throwing angle
        var angle = xForceScaled + yForce;
        // Apply force to the bomb
        GetComponent<Rigidbody2D>().AddForce(angle * speed, ForceMode2D.Impulse);
    }


    // Waiting Coroutines
    IEnumerator WaitAndExplode(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Explode();
    }

    IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);
    }

    // If bomb collides with enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<EnemyScript>();

        if (enemy != null)
        {
            // Deal impact damage once
            if (hasImpacted == false)
            {
                enemy.TakeDamage(impactDamage);
                hasImpacted = true;
            }
            
            Explode();
        }

        // If bomb collides with something else start timer and explode
        else
        {
            StartCoroutine(WaitAndExplode(explodeTime));
        }
    }

    private void Explode()
    {
        // Change to explosion sprite
        spriteRenderer.sprite = explosionSprite;

        // Resize sprite
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;  
        spriteRenderer.size = new Vector2(5,5);

        // Disable gravity while exploding
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Create explosion circle for the given range
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);

        foreach (var collision in hitColliders)
        {
            var enemy = collision.GetComponent<EnemyScript>();

            // If an enemy is in the explosion 
            if (enemy != null)
            {
                // Get distance between center of the explosion and enemy
                var closestPoint = collision.ClosestPoint(transform.position);
                var distance = Vector3.Distance(closestPoint, transform.position);

                // Calculate damage dealt based on distance
                var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                enemy.TakeDamage(damagePercent * explosionDamage);
            }

            // Destroy Secret Tile if it is in the explosion range
            if (collision.tag == "SecretTile")
            {
                Destroy(collision.gameObject);
            }
        }

        // Wait for 0.5 seconds to show explosion sprite and delete the bomb
        StartCoroutine(WaitAndDestroy(0.5f));
    }
}