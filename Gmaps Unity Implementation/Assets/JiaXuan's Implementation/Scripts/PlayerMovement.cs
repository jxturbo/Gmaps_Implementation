using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public ZeroGravity ZeroGravity;
    public float speed = 3f;
    public float gravity = 9.81f; 
    public float jumpHeight = 3f;
    private Vector3 gravityDirection = new Vector3(0, -1, 0); // Set gravity direction
    public Transform bottom;
    public Check check;
    Vector3 gravityVector = new Vector3(0f,0f,0f);
    Vector3 jumpVector;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (!ZeroGravity.noGravity)
        {
            //simple left right movement vector
            Vector3 move = transform.right * x + transform.forward * z;
            if (ZeroGravity.platformShift)
            {
                // Draw a ray from the player's position downwards referencing original rotation
                RaycastHit hit;
                Vector3 localDownVector = transform.TransformDirection(Vector3.down);
                gravityDirection = localDownVector;
                //this is basically the gravity under the influence of gravity platform shift
                //instead of drag down in world space
                //drags the player 'down' towards their current platform
                if (Physics.Raycast(transform.position, gravityDirection, out hit, Mathf.Infinity))
                {
                    // Set the opposite of the hit normal as the new gravity direction
                    //since hit.normal is perpendicular to the surface of the platform but facing upwards
                    gravityDirection = -hit.normal.normalized;
                    // Ensure the gravity vector points in the correct direction
                    gravityVector = gravityDirection * -gravity * rb.mass;
                    // Use Rigidbody.AddForce to apply force in the direction of gravity
                    Debug.DrawRay(transform.position, gravityDirection * 5f, Color.blue);
                }
                //this ensures that even if player leaves the platform, the gravity vector updates properly
                //eg:if player is upside down it will continue to go up even after being moving off the platform
                rb.AddForce(gravityVector, ForceMode.Force);
                //more or less done here in order to ensure player properly moves in a slanted platform
                move = Vector3.ProjectOnPlane(move, hit.normal).normalized;
                transform.Translate(move * speed * Time.deltaTime, Space.World);
            }
            else
            {
                //updates current gravity to just point down
                gravityDirection = Vector3.down;
                // Mimics constant gravity by pulling the player down
                Vector3 gravityVector = Vector3.down * -gravity * rb.mass;
                Debug.Log(gravityVector);
                // Use Rigidbody.AddForce to apply constant force in the downward direction
                rb.AddForce(gravityVector, ForceMode.Force);
                // Moves the player in accordance to the vector smoothly
                transform.Translate(move * speed * Time.deltaTime, Space.World);
                //if player falls down due to regular gravity, ensure that they always land on their feet
                //by ensuring their bottom is the one that hits the ground
                float uprightSpeed = 5.0f;
                Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, uprightRotation * rb.rotation, uprightSpeed * Time.deltaTime));
            
            }
            //checks if player is on a surface before letting them jump
            //so no infinite air jumps
            if (Input.GetButtonUp("Jump") && check.isGrounded)
            {
                // Calculate the jump velocity based on the new gravity direction/orignal direction
                float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(jumpHeight) * Mathf.Abs(gravity));
                //have it scale to mass in order to have it be adaptive
                //eg: if player is 100kg, they jump with more force than 1kg player to reach same height
                //(what we want: consistent jump height)
                jumpVelocity *= rb.mass;
                // Apply the jump velocity in the opposite direction of the gravity vector
                jumpVector = -gravityDirection * jumpVelocity;
                // Use Rigidbody.AddForce to apply the jump force
                rb.AddForce(jumpVector, ForceMode.Impulse);
                Debug.Log(jumpVector);
                check.isGrounded = false;
            }
        }
    }

}
