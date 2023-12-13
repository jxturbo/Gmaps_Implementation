using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirection : MonoBehaviour
{
    public PlayerMovement playerMovement;
    // Update is called once per frame
    void Update()
    {
        //more or less rotates the arrow sprite to point towards the player's current 'down'
        // Get the current gravity vector
        Vector3 gravityVector = playerMovement.gravityDirection;
        float rotationAngle = Mathf.Atan2(gravityVector.x, gravityVector.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }
}
