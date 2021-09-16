using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    // The name of the prefab that is used in the spawner instance
    public string prefabName;

    // The number of prefabs the spawner creates at once
    public int numberOfPrefabsToCreate;

    // The coordinates of possible spawn points
    public Vector3[] spawnPoints;
}