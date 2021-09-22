using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGEgg : MonoBehaviour
{
    // List of spawnable enemies
    [SerializeField]
    GameObject[] spawnableEnemies;

    // The delay after which the egg hatches
    [SerializeField]
    float spawnDelay;

    // Variables inside the script
    int randomNumber;
    float startingTime;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the enemy number and starting time
        randomNumber = Random.Range(0, spawnableEnemies.Length);
        startingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // When the delay has passed
        if (Time.time > startingTime + spawnDelay)
        {
            // Spawn an enemy and destroy the egg
            Instantiate(spawnableEnemies[randomNumber], transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}