using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;
    public GameObject selectedGameObject;

    //Basically how this code works is
    //1)draw ray from camera to player
    //2)check if anything intersects
    //2.1)if there is already a selected object, reset that first
    //3)grab that object and change its layer to one not rendered by the camera, effectively making it invisble
    //4)set it to a selected object variable to store the reference
    //5)if that object is not longer in the way, reset it to orignal layer that camera renders
    void Update()
    {
        //determines the vector used for the ray itself by drawing it from the camera to player
        Vector3 direction = playerTransform.position - cameraTransform.position;
        //offset here to make sure camera ray doesnt clash with the floor
        direction.y += 0.2f;
        // Set up a layer mask to ignore the player layer
        //so the layermask includes everything but the player since we want to check for anything between the camera and playyer
        LayerMask layerMask = ~LayerMask.GetMask("Player");
        //float variable used here to draw a ray that is exactly from the camera to the player with Physics.Raycast
        float distanceFromCameraToPlayer = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;
        //checks to see if the line of sight between camera and player is broken
        if (Physics.Raycast(cameraTransform.position, normalizedDirection, out RaycastHit hit, distanceFromCameraToPlayer, layerMask))
        {
            // Check if the hit object's layer is "Environment"
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                //check here to ensure that if we have an object that was previously
                //invisible, we change it back before doing everything else
                if(selectedGameObject != null)
                {
                    selectedGameObject.layer = LayerMask.NameToLayer("Environment");
                }
                // Change the hit object's layer to "Selected"
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Selected");
                selectedGameObject = hit.collider.gameObject;
            }
            Debug.DrawRay(cameraTransform.position,normalizedDirection,Color.blue , 5f);
        }
        //if object is not obstructing camera, reset it
        else if(selectedGameObject != null)
        {
            selectedGameObject.layer = LayerMask.NameToLayer("Environment");
        }
    }
}
