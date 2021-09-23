using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //player variables form PlayerContoller, must add everything that is wanted to save
    public float health;
    public float[] position;

    public PlayerData (PlayerController player)
    {
        //health = player.playerHealth;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
    }
}
