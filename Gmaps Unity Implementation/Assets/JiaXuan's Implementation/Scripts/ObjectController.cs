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
        if(noGravity)
        {
            rb.velocity = Vector3.zero;
        }
        if(this.tag != "Interacting")
        {
            if (inStasis)
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
        }

    }

    public void StartGravityStasisObject(Vector3 targetPosition, Vector3 upwardsVector)
    {
        StartCoroutine(GravityStasisObject(targetPosition, upwardsVector));
    }

    IEnumerator GravityStasisObject(Vector3 targetPosition, Vector3 upwardsVector)
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 initialPosition = rb.transform.position;
        // Ensure vectors are normalized
        Vector3 forwardVector = Vector3.Cross(rb.transform.right, upwardsVector).normalized;
        Quaternion destRot = Quaternion.LookRotation(forwardVector, upwardsVector);

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            rb.MovePosition(newPosition);

            // Rotate the Rigidbody to align with the downwards vector
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, destRot, (elapsedTime / duration) * 0.25f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        inStasis = true;
        this.tag = "Interactable";
    }
}
