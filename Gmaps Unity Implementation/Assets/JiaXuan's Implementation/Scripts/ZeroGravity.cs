using System.Collections;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    public Rigidbody rb;
    public float gravity = -9.81f;
    public bool noGravity;
    private bool gravityToggled;
    public MouseLook MouseLook;
    public bool isMoving = false;
    public bool platformShift;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            noGravity = !noGravity;
            if (noGravity)
            {
                rb.velocity = Vector3.zero;
                // Move the person up by 2.5 and effectively prevents them from falling back down
                rb.transform.Translate(rb.transform.up * 2.5f, Space.World);
                playerMovement.enabled = false; 
            }
            else if (!noGravity)
            {
                platformShift = false;
            }
        }

        if (noGravity)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                // Get the mouse position in screen space
                Vector3 mousePosition = Input.mousePosition;
                // Create a ray from the center of the screen forward until it hits a surface
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                // Define a layer mask to include only the "Environment" layer
                int environmentLayerMask = 1 << LayerMask.NameToLayer("Environment");
                // To store anything it hits
                RaycastHit hit;
                // Perform the raycast with the specified layer mask
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, environmentLayerMask))
                {
                    // Get the upwardsVector vector based on the normal of the hit point
                    Vector3 upwardsVector = hit.normal;
                    // Start moving the Rigidbody to the hit point
                    StartCoroutine(GravityShift(hit.point, upwardsVector));
                    // Log the name of the object that was hit
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

    IEnumerator GravityShift(Vector3 targetPosition, Vector3 upwardsVector)
    {
        isMoving = true;
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 initialPosition = rb.transform.position;

        // Ensure vectors are normalized
        Debug.Log(rb.transform.right);
        Vector3 forwardVector = Vector3.Cross(rb.transform.right, upwardsVector).normalized;
        Quaternion destRot = Quaternion.LookRotation(forwardVector, upwardsVector);

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            rb.MovePosition(newPosition);

            // Rotate the Rigidbody to align with the downwards vector
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, destRot, (elapsedTime / duration) * 0.5f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        noGravity = false;
        platformShift = true;
        playerMovement.enabled = true;
    }
}

