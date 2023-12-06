using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;
    public GameObject selectedGameObject;

    // Update is called once per frame
    void Update()
    {
        //determines the vector used for the ray itself from the camera to player
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
        else if(selectedGameObject != null)
        {
            selectedGameObject.layer = LayerMask.NameToLayer("Environment");
        }
    }
}
