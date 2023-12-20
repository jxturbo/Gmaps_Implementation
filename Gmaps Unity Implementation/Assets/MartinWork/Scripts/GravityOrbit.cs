using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbit : MonoBehaviour
{
    public float gravity;
    public bool fixedDirection; // If gravity pushes player only down
     
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

            // Check if the current gravity is the same as the exiting gravity field
            if (gravityControl.gravity == this.GetComponent<GravityOrbit>())
            {
                gravityControl.gravity = null; // Reset gravity when leaving the zone
            }
        }
    }

}
