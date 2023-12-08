using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Rigidbody rb; // Reference to the Rigidbody
    public ZeroGravity ZeroGravity;
    public bool rotationReset;
    public bool DetachCamera;
    public Transform cameraHolder;
    float xRotation = 0f;
    private bool LookAtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //locks cursor and makes it invisible to prevent it from yeeting off the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DetachCamera = !DetachCamera;
        }
        if (ZeroGravity.noGravity && !ZeroGravity.isMoving)
        {
            // Takes in vertical and horizontal mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the player's body (or another parent object) around the y-axis based on mouseX
            playerBody.Rotate(Vector3.up * mouseX);
            // Rotate the player's body (or another parent object) around the x-axis based on mouseY
            playerBody.Rotate(Vector3.right * mouseY);

            // Rotate the Rigidbody around the y-axis based on mouseX
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
            // Rotate the Rigidbody around the x-axis based on mouseY
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.right * mouseY));
        }
        else
        {
            // Takes in vertical and horizontal mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            if (rotationReset)
            {
                playerBody.rotation = Quaternion.identity;
                rotationReset = false;
            }
            xRotation -= mouseY;
            //restrictions player to looking 90 degrees up and down
            xRotation = Mathf.Clamp(xRotation, -45f, 45f);
            // Check if the camera is detached
            if (DetachCamera)
            {
                // Rotate the player camera around the player (no effect on the body)
                transform.RotateAround(playerBody.position, Vector3.up, mouseX);
                // Calculate the rotation based on the vertical axis input
                Quaternion verticalRotation = Quaternion.Euler(xRotation, 0f, 0f);
                // Apply the vertical rotation to the camera
                transform.rotation = Quaternion.LookRotation(playerBody.position - transform.position) * verticalRotation;
                // Set LookAtPlayer to false
                LookAtPlayer = false;
            }
            else
            {
                // If the camera is not detached, look at the player once and then continue with regular rotation
                if (!LookAtPlayer)
                {
                    // Look at the player
                    transform.position = cameraHolder.position;
                    transform.LookAt(playerBody.position);
                    LookAtPlayer = true; 
                }
                else
                {
                    // Rotate the player camera to mouse input
                    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                    // Rotate the player's body (or another parent object) around the y-axis based on mouseX
                    playerBody.Rotate(Vector3.up * mouseX);
                    // Rotate the Rigidbody around the y-axis based on mouseX
                    rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
                }
            }


        }
    }
}
