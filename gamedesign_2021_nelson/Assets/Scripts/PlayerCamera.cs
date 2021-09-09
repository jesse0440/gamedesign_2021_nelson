using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Player variable
    public Transform player_location;

    // Assign the player variable
    void Start() 
    {
        player_location = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Update camera position based on player position
        transform.position = new Vector3(player_location.position.x, transform.position.y, transform.position.z);
    }
}
