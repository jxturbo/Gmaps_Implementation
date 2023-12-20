using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbit : MonoBehaviour
{
    public float gravity;

    // If gravity pushes player only down
    public bool fixedDirection; 
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravityControl>())
        {
            other.GetComponent<GravityControl>().gravity = this.GetComponent<GravityOrbit>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GravityControl>())
        {
            GravityControl gravityControl = other.GetComponent<GravityControl>();

            // Checks if the current gravity is the same as new gravity field
            if (gravityControl.gravity == this.GetComponent<GravityOrbit>())
            {
                // Reset gravity when leaving the gravity field
                gravityControl.gravity = null; 
            }
        }
    }

}
