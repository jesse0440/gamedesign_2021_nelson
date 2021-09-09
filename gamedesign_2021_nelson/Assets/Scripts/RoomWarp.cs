using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomWarp : MonoBehaviour
{
    // The ID of the next room
    // Needs to be entered manually for each trigger!!
    public int next_room_id;

    // When entering the trigger
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Load next room
        SceneManager.LoadScene(next_room_id);
    }
}