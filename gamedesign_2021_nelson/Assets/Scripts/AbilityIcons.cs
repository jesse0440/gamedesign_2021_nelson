using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcons : MonoBehaviour
{
    // Variables for the ability icons
    public GameObject abilityDoubleJump;
    public GameObject abilityWallClimb;
    public GameObject abilityDash;
    public GameObject abilityTeleport;

    public Text dashInterval;
    public Text teleportInterval;

    // Timers for abilities
    float dashTime;
    float teleportTime;

    // The player's script
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Assign ability icons
        abilityDoubleJump = GameObject.FindWithTag("DoubleJump");
        abilityWallClimb = GameObject.FindWithTag("WallClimb");
        abilityDash = GameObject.FindWithTag("Dash");
        abilityTeleport = GameObject.FindWithTag("Teleport");

        // Assign ability interval icons
        dashInterval = GameObject.Find("DashInterval").GetComponent<Text>();
        teleportInterval = GameObject.Find("TeleportInterval").GetComponent<Text>();

        // Assign player
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // If Ability X is unlocked show the icon
        // Otherwise hide the icon
        if (PlayerPrefs.GetFloat("Ability_0", 1) == 1)
        {
            abilityDoubleJump.SetActive(false);
        }

        if (PlayerPrefs.GetFloat("Ability_1", 0) == 0)
        {
            abilityWallClimb.SetActive(false);
        }

        if (PlayerPrefs.GetFloat("Ability_2", 0) == 0)
        {
            abilityDash.SetActive(false);
            dashInterval.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetFloat("Ability_3", 0) == 0)
        {
            abilityTeleport.SetActive(false);
            teleportInterval.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Update dash cooldown on the screen every frame
        dashTime = (float)Math.Round(playerController.dashIntervalTimer + playerController.dashInterval - Time.time, 1);
        dashInterval.text = "" + dashTime;

        // But do not go to the negative values
        if (dashTime < 0f)
        {
            dashInterval.text = "0";
        }

        // Change later
        teleportInterval.text = "0";
    }
}