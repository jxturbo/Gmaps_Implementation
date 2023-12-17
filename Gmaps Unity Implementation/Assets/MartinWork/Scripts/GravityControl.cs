using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public GravityOrbit gravity;
    private Rigidbody rb;

    public float rotationSpeed = 20f;
    private bool inGravityField = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        // Checks for gravity 
        if (gravity) 
        {
            Vector3 gravityUp = Vector3.zero;

            if (gravity.fixedDirection)
            {
                // If fixed direction, uses colliding orbit
                gravityUp = gravity.transform.up; 
            }
            else
            {
                gravityUp = (transform.position - gravity.transform.position).normalized; //  
            } 

            // Player's upward direction
            Vector3 localUp = transform.up;
            
            // Makes it so that the player slowly rotate overtime to adjust to new gravity
            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 

            rb.AddForce((-gravityUp * gravity.gravity) * rb.mass);

            // Switches off gravity in Rigidbody when in field
            inGravityField = true;
            rb.useGravity = false; 
        }
        else if (inGravityField)
        {
            // Switches on gravity in Rigidbody when in field
            rb.useGravity = true; 
            inGravityField = false;
        }
    }
}
