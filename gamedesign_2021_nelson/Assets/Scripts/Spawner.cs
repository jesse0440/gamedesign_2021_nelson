using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    // An instance of the ScriptableObject defined above.
    public SpawnManagerScriptableObject spawnManagerValues;

    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;

    // Bool to determine if this enemy can detect and charge at the player
    [SerializeField]
    bool canEnemyDetectPlayer;

    // The range in which the enemy can detect the player
    [SerializeField]
    float playerDetectionRange;

    // Transform of the player
    Transform playerTransform;

    double spawning1;

    void FixedUpdate()
    {
        if (canEnemyDetectPlayer)
        {
            // Find the player's transform and the distance between the enemy and the player
            playerTransform = GameObject.FindWithTag("Player").transform;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= playerDetectionRange)
            {
                spawning1 = spawning1 + 0.5;
            }

            if (spawning1 == 100 | spawning1 == 200 | spawning1 == 300)
            {
                SpawnEntities();
            }

            if (distanceToPlayer > playerDetectionRange)
            {
                spawning1 = 0;
            }

        }
    }

    void SpawnEntities()
    {
        int currentSpawnPointIndex = 0;

        for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
        {
            // Creates an instance of the prefab at the current spawn point.
            GameObject currentEntity = Instantiate(entityToSpawn, spawnManagerValues.spawnPoints[currentSpawnPointIndex], Quaternion.identity);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = spawnManagerValues.prefabName + instanceNumber;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

            instanceNumber++;
        }
    }
}
