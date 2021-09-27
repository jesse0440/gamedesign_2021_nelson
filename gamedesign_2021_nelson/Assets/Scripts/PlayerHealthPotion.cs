using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthPotion : MonoBehaviour
{
    // Field to determine how much health is replenished
    public float healthReplenished;

    // Start is called before the first frame update
    void Start()
    {
        if (healthReplenished == 0f)
        {
            healthReplenished = 50f;
        }
    }
}