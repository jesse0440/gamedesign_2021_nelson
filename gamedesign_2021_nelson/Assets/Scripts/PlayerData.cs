using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    // Game variables that need to be saved for when the game is loaded next
    [HideInInspector]
    public float savedPlayerHealth;
    [HideInInspector]
    public float savedPlayerMaxHealth;
    [HideInInspector]
    public float[] savedPlayerPosition;
    [HideInInspector]
    public float savedAttackRange;
    [HideInInspector]
    public float savedMeleeDamage;
    [HideInInspector]
    public float savedMeleeAttackInterval;
    [HideInInspector]
    public float savedShurikenAttackInterval;
    [HideInInspector]
    public float savedBombAttackInterval;
    [HideInInspector]
    public float savedPlayerMaxJumpCounter;
    [HideInInspector]
    public int savedCurrentShuriken;
    [HideInInspector]
    public int savedCurrentBombs;
    [HideInInspector]
    public int savedMaxBombs;
    [HideInInspector]
    public int savedMaxShuriken;
    [HideInInspector]
    public float savedWallClimbValue;
    [HideInInspector]
    public float savedDashUnlockedCheck;
    [HideInInspector]
    public float savedDashDistance;
    [HideInInspector]
    public float savedDashInterval;
    [HideInInspector]
    public float savedTeleportUnlockedCheck;
    [HideInInspector]
    public float savedTeleportInterval;
    [HideInInspector]
    public int savedYellowCount;
    [HideInInspector]
    public int savedBlueCount;
    [HideInInspector]
    public int savedRedCount;

    [HideInInspector]
    public int savedSceneNumber;

    public int firstTime;

    // This data will be saved
    public PlayerData(PlayerController player)
    {
        // Saved data variables
        savedPlayerHealth = player.playerHealth;
        savedPlayerMaxHealth = player.playerMaxHealth;

        savedPlayerPosition = new float[2];
        savedPlayerPosition[0] = player.transform.position.x;
        savedPlayerPosition[1] = player.transform.position.y;

        savedAttackRange = player.attackRange;
        savedMeleeDamage = player.meleeDamage;
        savedMeleeAttackInterval = player.meleeAttackInterval;
        savedShurikenAttackInterval = player.shurikenAttackInterval;
        savedBombAttackInterval = player.bombAttackInterval;
        savedPlayerMaxJumpCounter = player.playerMaxJumpCounter;
        savedCurrentShuriken = player.currentShuriken;
        savedCurrentBombs = player.currentBombs;
        savedMaxBombs = player.maxBombs;
        savedMaxShuriken = player.maxShuriken;
        savedWallClimbValue = player.wallClimbValue;
        savedDashUnlockedCheck = player.dashUnlockedCheck;
        savedDashDistance = player.dashDistance;
        savedDashInterval = player.dashInterval;
        savedTeleportUnlockedCheck = player.teleportUnlockedCheck;
        savedTeleportInterval = player.teleportInterval;
        savedYellowCount = player.yellowCount;
        savedBlueCount = player.blueCount;
        savedRedCount = player.redCount;

        firstTime = 0;

        savedSceneNumber = SceneManager.GetActiveScene().buildIndex;
    }
}