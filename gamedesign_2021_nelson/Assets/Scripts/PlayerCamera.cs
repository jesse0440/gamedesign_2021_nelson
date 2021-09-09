using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Player transform
    public Transform player_location;

    // Camera boundaries
    // Needs to be entered manually in the editor for each individual room!!
    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float topLimit;

    // Assign the player variable
    void Start() 
    {
        player_location = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Update camera position based on player position
        // vvvvv Old method, leaving here just in case
        // transform.position = new Vector3(player_location.position.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(player_location.position.x, player_location.position.y, transform.position.z);

        // Clamp the edges of the camera to the set limits
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
            transform.position.z
        );
    }

    // An outline of the size of the camera boundaries in the level
    // Uncomment if you need to edit some boundaries

    /*

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, bottomLimit), new Vector2(rightLimit, topLimit));
    }
    
    */

}