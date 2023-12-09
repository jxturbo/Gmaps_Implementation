using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public bool inStasis;
    private Rigidbody rb;
    public float gravity = -9.81f; 
    private Vector3 gravityDirection = new Vector3(0, -1, 0); // Set gravity direction
    Vector3 gravityVector = new Vector3(0f,0f,0f);
    public bool noGravity;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();        
    }
    void Update()
    {
        //basically make sure that object floats in zero gravity and can't move
        if(noGravity)
        {
            rb.velocity = Vector3.zero;
        }
        //interacting is used to show if object is in stasis
        if(this.tag != "Interacting")
        {
            if (inStasis)
            {
                // Draw a ray from the object's position in the direction of the current gravity
                RaycastHit hit;
                Vector3 localDownVector = transform.TransformDirection(Vector3.down);
                gravityDirection = localDownVector;
                //this is basically the gravity under the influence of stasis
                //instead of drag down in world space
                //drags the object 'down' towards their current platform
                if (Physics.Raycast(transform.position, gravityDirection, out hit, Mathf.Infinity))
                {
                    // Set the opposite of the hit normal as the new gravity direction
                    gravityDirection = -hit.normal.normalized;
                    // Ensure the gravity vector points in the correct direction
                    gravityVector = gravityDirection * -gravity * rb.mass;
                    // Use Rigidbody.AddForce to apply force in the direction of gravity
                    Debug.DrawRay(transform.position, gravityDirection * 5f, Color.blue);
                }
                //this ensures that even if object leaves the platform, the gravity vector updates properly
                //eg:if object is upside down it will continue to go up even after being pushed off the platform
                rb.AddForce(gravityVector, ForceMode.Force);
            }
            else
            {
                // Mimics constant [Regular] gravity by pulling the object down
                Vector3 gravityVector = Vector3.down * -gravity * rb.mass;;
                // Use Rigidbody.AddForce to apply constant force in the downward direction
                rb.AddForce(gravityVector, ForceMode.Force);
            }
        }

    }
    //done here because courtines cant be run from other scripts
    //especially if its using a for loop
    public void StartGravityStasisObject(Vector3 targetPosition, Vector3 upwardsVector)
    {
        StartCoroutine(StasisBlast(targetPosition, upwardsVector));
    }
    //uses cross product to ensure 3 things
    //1)object moves towards the destination the player was pointing it when they clicked the mouse(stasis blast)
    //2)object's feet/bottom is what lands on the surface, not any other side
    //3)object is pointing forward instead of backwards(more needed for player to prevent them from spinning too much)
    IEnumerator StasisBlast(Vector3 targetPosition, Vector3 upwardsVector)
    {
        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 initialPosition = rb.transform.position;
        // Ensure vectors are normalized
        Vector3 forwardVector = Vector3.Cross(rb.transform.right, upwardsVector).normalized;
        Quaternion destRot = Quaternion.LookRotation(forwardVector, upwardsVector);
        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            rb.MovePosition(newPosition);
            // Rotate the Rigidbody to align with the resultant rotation
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, destRot, (elapsedTime / duration) * 0.25f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        inStasis = true;
        //resets object to make it no longer under the influence of player but still in stasis
        //so unaffected by initial gravity but cannot be controlled by player
        this.tag = "Interactable";
    }
}
