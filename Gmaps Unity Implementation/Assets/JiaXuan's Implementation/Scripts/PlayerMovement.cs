using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public ZeroGravity ZeroGravity;
    public float speed = 3f;
    public float gravity = -9.81f; 
    public float jumpHeight = 3f;
    private Vector3 gravityDirection = new Vector3(0, 1, 0); // Set gravity direction
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
            Vector3 move = transform.right * x + transform.forward * z;

            // Move the player
            transform.Translate(move * speed * Time.deltaTime, Space.World);

            if (ZeroGravity.platformShift)
            {
                // Draw a ray from the player's position in the direction of the current gravity
                RaycastHit hit;
                Vector3 localDownVector = transform.TransformDirection(Vector3.down);
                gravityDirection = localDownVector;
                if (Physics.Raycast(transform.position, gravityDirection, out hit, Mathf.Infinity))
                {
                    // Set the opposite of the hit normal as the new gravity direction
                    gravityDirection = -hit.normal.normalized;
                    // Ensure the gravity vector points in the correct direction
                    gravityVector = gravityDirection * -gravity * rb.mass;
                    // Use Rigidbody.AddForce to apply force in the direction of gravity
                    Debug.DrawRay(transform.position, gravityDirection * 5f, Color.blue);
                }
                rb.AddForce(gravityVector, ForceMode.Force);
            }
            else
            {
                // Mimics constant gravity by pulling the player down
                Vector3 gravityVector = Vector3.down * -gravity * rb.mass;;
                // Use Rigidbody.AddForce to apply constant force in the downward direction
                rb.AddForce(gravityVector, ForceMode.Force);
            }

            if (Input.GetButtonUp("Jump") && check.isGrounded)
            {
                
                // Calculate the jump velocity based on the new gravity direction
                float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(jumpHeight) * Mathf.Abs(-gravity));
                // Apply the jump velocity in the direction of the gravity vector
                if(ZeroGravity.platformShift)
                {
                    jumpVector = -gravityDirection * jumpVelocity;
                }
                else
                {
                    jumpVector = gravityDirection * jumpVelocity;
                }
                // Use Rigidbody.AddForce to apply the jump force
                rb.AddForce(jumpVector, ForceMode.Impulse);
                Debug.Log(jumpVector);
                // Move this line to ensure isGrounded is set after the jump force is applied
                check.isGrounded = false;
                
            }




        }
    }

}
