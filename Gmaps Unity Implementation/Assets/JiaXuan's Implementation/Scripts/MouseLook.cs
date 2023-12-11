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
        //Mostly here to toggle the cursor on and off to allow player to close the game if needed
        //more or less the same for time since my camera script follows the mouse 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if its locked, unlock it, if not, lock it pretty much
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
            //if frozen, unfreeze, same as cursor
            Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;
        }
        //here mostly just to prevent this particular case
        //since player and camera always face forward, 
        //all the objects they will make in stasis will try to squeeze into the same space
        //this ensures that the camera can move, stasis and object, rotate and stasis another in a [Different] spot
        // Takes in vertical and horizontal mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DetachCamera = !DetachCamera;
        }
        //this basically allows the player to rotate freely on all three axis when there is zero gravity
        if (ZeroGravity.noGravity && !ZeroGravity.isMoving)
        {
            playerBody.Rotate(Vector3.up * mouseX);
            playerBody.Rotate(Vector3.right * mouseY);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.right * mouseY));
        }
        else
        {
            //resets player rotation when transitioning from detached camera to not a detached camera
            if (rotationReset)
            {
                playerBody.rotation = Quaternion.identity;
                rotationReset = false;
            }
            xRotation -= mouseY;
            //restrictions player to looking 90 degrees up and down
            xRotation = Mathf.Clamp(xRotation, -45f, 45f);
            // Check if the camera is detached
            //if so, allow the camera to rotate around the player while facing it as well as rotating up and down
            //all done without moving the player
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
                // Check if the camera is detached
                //if so, allow the camera to rotate around the player while facing it as well as rotating up and down
                //all done while rotating the player to be facing the same direction as the camera
                if (!LookAtPlayer)
                {
                    // Look at the player once just to anchor the camera properly when returning from detached
                    //to attached
                    transform.position = cameraHolder.position;
                    transform.LookAt(playerBody.position);
                    LookAtPlayer = true; 
                }
                else
                {
                    // Rotate the player camera in accordance to mouse input
                    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                    playerBody.Rotate(Vector3.up * mouseX);
                    rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * mouseX));
                }
            }


        }
    }
}
