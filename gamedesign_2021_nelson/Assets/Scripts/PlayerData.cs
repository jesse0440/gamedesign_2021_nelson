using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    // Game variables that need to be saved for when the game is loaded next
    public float savedPlayerHealth;
    public float savedPlayerMaxHealth;
    public float[] savedPlayerPosition;
    public float savedAttackRange;
    public float savedMeleeDamage;
    public float savedMeleeAttackInterval;
    public float savedRangedAttackInterval;
    public float savedPlayerMaxJumpCounter;
    public int savedSceneNumber;


    void Start()
    {
        // Assign the player object and script
        //playerObject = GameObject.FindWithTag("Player");
        //playerController = playerObject.GetComponent<PlayerController>();
    }

    // This data will be saved
    public PlayerData(PlayerController player)
    {
        //Saved data variables
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