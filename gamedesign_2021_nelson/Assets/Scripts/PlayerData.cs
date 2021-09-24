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
    public float savedRangedAttackInterval;
    [HideInInspector]
    public float savedPlayerMaxJumpCounter;
    [HideInInspector]
    public int savedSceneNumber;

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
        savedRangedAttackInterval = player.rangedAttackInterval;
        savedPlayerMaxJumpCounter = player.playerMaxJumpCounter;

        savedSceneNumber = SceneManager.GetActiveScene().buildIndex;
    }
}