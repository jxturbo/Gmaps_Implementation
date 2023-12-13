using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public GravityOrbit gravity; 
    private Rigidbody rb;

    public float rotationSpeed = 20f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Reference the object rigidbody
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gravity) // Checks if there is gravity 
        {
            Vector3 gravityUp = Vector3.zero;

            if (gravity.fixedDirection)
            {
                gravityUp = gravity.transform.up; // If fixed direction, uses colliding orbit
            }
            else
            {
                gravityUp = (transform.position - gravity.transform.position).normalized; //  
            }
            Vector3 localUp = transform.up;  
            // Player's upward direction
             
            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation; 
            //  

            transform.up = Vector3.Lerp(transform.up, gravityUp, rotationSpeed * Time.deltaTime);
            // Makes it so that the player slowly rotate overtime to adjust to new gravity 

            rb.AddForce((-gravityUp * gravity.gravity) * rb.mass);

        }
    }
}
