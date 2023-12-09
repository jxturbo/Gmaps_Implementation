using System.Collections;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    public Rigidbody rb;
    public bool noGravity;
    private bool gravityToggled;
    public MouseLook MouseLook;
    public bool isMoving = false;
    public bool platformShift;
    public PlayerMovement playerMovement;
    public GravityStasis gravityStasis;

    void Update()
    {
        //basically make sure that player floats in zero gravity and can't move
        if(noGravity)
        {
            rb.velocity = Vector3.zero;
        }
        //to trigger zero gravity
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
            //this specific code is to reset player if it was under zero gravity
            //and allow it to function normally under regular gravity(custom and not the rigidbody's gravity)
            else if (!noGravity)
            {
                platformShift = false;
                playerMovement.enabled = true; 
            }
        }
        if (noGravity)
        {
            //draws a ray forward from mouse position(better accuracy) and checks if it collides
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                // Get the mouse position in screen space
                Vector3 mousePosition = Input.mousePosition;
                // Create a ray from the center of the screen to its forward until it hits a surface
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                //checks for only legal surfaces player can land on, not a box for example
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
    //uses cross product to ensure 3 things
    //1)player moves towards the destination the player was pointing it when they clicked the mouse(stasis blast)
    //2)player's feet/bottom is what lands on the surface, not any other side
    //3)player is pointing forward instead of backwards needed to smooth out rotation and ensure they dont rotate 180 degrees
    //potentiall disorienting the player
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
            this.transform.Translate(newPosition - rb.transform.position, Space.World);
            // Rotate the Rigidbody to align with the downwards vector
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, destRot, (elapsedTime / duration) * 0.25f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //player is no longer under zero gravity but still has gravity powers enabled
        //so their new gravity is using the platform they are now on as refernece for 'down'
        isMoving = false;
        noGravity = false;
        platformShift = true;
        playerMovement.enabled = true;
    }
}

