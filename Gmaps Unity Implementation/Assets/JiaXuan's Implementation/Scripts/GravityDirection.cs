using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirection : MonoBehaviour
{
    public Transform imageTransform;
    public PlayerMovement playerMovement;
    // Update is called once per frame
    void Update()
    {
        // Get the current gravity vector
        Vector3 gravityVector = playerMovement.gravityDirection;
        float rotationAngle = Mathf.Atan2(gravityVector.x, gravityVector.y) * Mathf.Rad2Deg;
        imageTransform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }
}
