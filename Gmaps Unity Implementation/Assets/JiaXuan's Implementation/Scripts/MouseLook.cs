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
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //locks cursor and makes it invisible to prevent it from yeeting off the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
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
            //rotates player camera to mouse input
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // Rotate the player's body (or another parent object) around the y-axis based on mouseX
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate the Rigidbody around the y-axis based on mouseX
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
        }
    }
}
