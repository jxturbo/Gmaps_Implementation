using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class CameraControl : MonoBehaviour
{
    public Transform playerTransform;
    public LayerMask layerMask;

    public float distance = 5.0f; // Distance between CAMERA & PLAYER
    public float height = 2.0f; // Height of the CAMERA above PLAYER
    public float smoothSpeed = 5f; // Smoothing value for CAMERA movement

    public float rotationSpeed = 1.0f; // Speed of rotation around the PLAYER
    public float ignoreDist = 1.0f; // Distance to avoid collisions
    public float minDistPlayer = 2.0f; // Minimum distance between CAMERA & PLAYER

    private Vector3 targetPos;

    private void Update()
    {
        if (playerTransform != null)
        {
            CamRotation();
            RepositionCam();
        }
    }

    private void CamRotation()
    {
        transform.RotateAround(playerTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void RepositionCam()
    {
        // Calculate CAMERA target position 
        targetPos = playerTransform.position - playerTransform.forward * distance + Vector3.up * height;

        RaycastHit hit;
        Vector3 rayDir = (targetPos - playerTransform.position).normalized;
        float maxDist = (targetPos - playerTransform.position).magnitude - ignoreDist;

        // Raycast to check for collisions
        if (Physics.Raycast(playerTransform.position, rayDir, out hit, maxDist, layerMask))
        {
            // Repositions the CAMERA slightly forward
            Vector3 newPos = hit.point - rayDir * ignoreDist;
            targetPos = newPos;

            // Checks if distance between CAMERA and PLAYER is < Minimum
            float currentDist = Vector3.Distance(targetPos, playerTransform.position);
            if (currentDist < minDistPlayer)
            {
                // Move the CAMERA away from the player 
                targetPos += (targetPos - playerTransform.position).normalized * (minDistPlayer - currentDist);
            }
        }

        // Moves CAMERA smoothly to target position
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
        transform.LookAt(playerTransform);
    }
}
