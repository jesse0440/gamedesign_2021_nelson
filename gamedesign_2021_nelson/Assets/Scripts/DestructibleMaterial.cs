using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructibleMaterial : MonoBehaviour
{
    public int destructibleIDInRoom;

    int hasBeenDestroyed = 0;

    // The ID of the room the container is located in
    int roomID;

    void Start()
    {
        roomID = SceneManager.GetActiveScene().buildIndex;

        hasBeenDestroyed = PlayerPrefs.GetInt("Destroyed_" + roomID + "_" + destructibleIDInRoom, 0);

        if (hasBeenDestroyed == 1)
        {
            gameObject.SetActive(false);
        }
    }
}