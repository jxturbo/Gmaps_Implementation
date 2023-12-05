using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    float OriginalCameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        //locks cursor and makes it invisible to prevent it from yeeting off the screen
        Cursor.lockState = CursorLockMode.Locked;
        OriginalCameraHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Takes in vertical and horizontal mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the rotation around the x-axis based on mouseY
        xRotation -= mouseY;

        // Restrict the rotation to be between -90 and 90 degrees
        xRotation = Mathf.Clamp(xRotation, -45f, 90f);

        // Calculate the rotation for the camera around the x-axis
        Quaternion newRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player's body (or another parent object) around the y-axis based on mouseX
        playerBody.Rotate(Vector3.up * mouseX);

        // Apply the rotation to the camera's local rotation
        transform.localRotation = newRotation;

        // Calculate the camera's position based on the rotation around the x-axis
        float cameraDistance = Mathf.Lerp(-5f, 0f, Mathf.InverseLerp(0f, 90f, Mathf.Abs(xRotation)));
        float cameraHeight = Mathf.Lerp(2f, OriginalCameraHeight, Mathf.InverseLerp(0f, 90f, Mathf.Abs(xRotation)));
        Vector3 newCameraPosition = new Vector3(0f, cameraHeight, cameraDistance);

        // Apply the new camera position
        transform.localPosition = newCameraPosition;



    }
}
