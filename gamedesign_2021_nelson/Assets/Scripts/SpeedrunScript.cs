using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;

    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        if (gameManagerScript.secretBossBeaten == false)
        {
            gameObject.SetActive(false);
        }
    }
}