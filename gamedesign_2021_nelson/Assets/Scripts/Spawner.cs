using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The GameObject to instantiate
    public GameObject entityToSpawn;

    // An instance of the ScriptableObject defined above
    public SpawnManagerScriptableObject spawnManagerValues;

    // This will be appended to the name of the created entities and increment when each is created
    int instanceNumber = 0;

    // Bool to determine if this enemy can detect and charge at the player
    [SerializeField]
    bool canEnemyDetectPlayer;

    // The range in which the enemy can detect the player
    [SerializeField]
    float playerDetectionRange;

    // Transform of the player
    Transform playerTransform;

    // The ticker that determines if another enity can be spawned
    double entitySpawnTicker;

    // The tipping point of the ticker
    [SerializeField]
    double entitySpawnTickerMax;

    // The distance between the spawner and the player
    float distanceToPlayer;

    // Fixed update is called at the same frame regardless of framerate
    void FixedUpdate()
    {
        // If the spawner has the ability to detect a player
        if (canEnemyDetectPlayer)
        {
            // Find the player's transform and the distance between the enemy and the player
            playerTransform = GameObject.FindWithTag("Player").transform;
            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // If the player is closer than the spawner's range
            if (distanceToPlayer <= playerDetectionRange)
            {
                // Increase ticker
                entitySpawnTicker += 0.5d;
            }

            // If the player moves out of the spawner's range
            else if (distanceToPlayer > playerDetectionRange)
            {
                // Reset the ticker
                entitySpawnTicker = 0d;
            }

            // If the ticker reaches or surpasses the tipping point
            if (entitySpawnTicker >= entitySpawnTickerMax)
            {
                // Spawn an entity and reset the ticker
                SpawnEntities();
                entitySpawnTicker = 0d;
            }
        }
    }

    // Entity spawning function
    void SpawnEntities()
    {
        // The index of the entity that will be spawned first
        int currentSpawnPointIndex = 0;

        // For loop to spawn a predetermined number of prefabs
        for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
        {
            // Creates an instance of the prefab at the current spawn point.
            GameObject currentEntity = Instantiate(entityToSpawn, spawnManagerValues.spawnPoints[currentSpawnPointIndex], Quaternion.identity);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = spawnManagerValues.prefabName + "_" + instanceNumber;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

            // The spawnable entity instance number goes up by one
            instanceNumber++;
        }
    }
}