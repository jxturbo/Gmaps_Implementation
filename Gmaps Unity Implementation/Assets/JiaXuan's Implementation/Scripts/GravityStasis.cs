using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityStasis : MonoBehaviour
{
    public bool stasisActive;
    public bool objectInStasis;
    private bool objectIsMoving;
    public Transform orignalObjectParent;
    List<Coroutine> GravityStasisCoroutines = new List<Coroutine>();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && !objectIsMoving)
        {
            stasisActive = !stasisActive;
            if(!stasisActive)
            {
                GameObject[] ObjectsInStasis = GameObject.FindGameObjectsWithTag("Interactable");
                foreach (GameObject obj in ObjectsInStasis)
                {
                    ObjectController objectController = obj.GetComponent<ObjectController>();
                    objectController.inStasis = false;
                    
                }
                GameObject[] ObjectsAttachedToPlayer = GameObject.FindGameObjectsWithTag("Interacting");
                foreach (GameObject obj in ObjectsAttachedToPlayer)
                {
                    obj.transform.SetParent(orignalObjectParent);
                    obj.tag = "Interactable";
                    ObjectController objectController = obj.GetComponent<ObjectController>();
                    objectController.noGravity = false;
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && stasisActive)
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;
            // Create a ray from the center of the screen forward until it hits a surface
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            // To store anything it hits
            RaycastHit hit;
            int noPlayerLayerMask = ~(1 << LayerMask.NameToLayer("Player"));
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, noPlayerLayerMask) && hit.collider.CompareTag("Interactable"))
            {
                Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                ObjectController currentObjectController = hit.collider.gameObject.GetComponent<ObjectController>();
                currentObjectController.noGravity = true;
                hit.collider.gameObject.tag = "Interacting";
                hit.collider.gameObject.transform.SetParent(this.gameObject.transform);
                rb.velocity = Vector3.zero;
                // Move the person up by 2.5 and effectively prevents them from falling back down
                rb.transform.Translate(rb.transform.up * 2f, Space.World);
                Debug.Log(rb.velocity);
                
            }
        }
        else if(Input.GetMouseButtonDown(1) && stasisActive && !objectIsMoving)
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;
            // Create a ray from the center of the screen forward until it hits a surface
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            // Define a layer mask to include only the "Player" layer
            int PlayerLayerMask = ~ (1 << LayerMask.NameToLayer("Player"));
            // To store anything it hits
            RaycastHit hit;
            // Perform the raycast with the specified layer mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerLayerMask))
            {
                // Get the upwardsVector vector based on the normal of the hit point
                Vector3 upwardsVector = hit.normal;
                // Start moving the Rigidbody to the hit point
                GravityStasisObjectAll(hit.point, upwardsVector);
                // Log the name of the object that was hit
                Debug.Log(hit.collider.gameObject.name);
            }          
        }       
    }


    public void GravityStasisObjectAll(Vector3 targetPosition, Vector3 upwardsVector)
    {
        objectIsMoving = true;
        // Find all objects with the tag "Interacting"
        GameObject[] interactingObjects = GameObject.FindGameObjectsWithTag("Interacting");
        foreach (GameObject obj in interactingObjects)
        {
            ObjectController objectController = obj.GetComponent<ObjectController>();
            if (objectController != null)
            {
                obj.transform.SetParent(orignalObjectParent);
                objectController.noGravity = false;
                objectController.StartGravityStasisObject(targetPosition, upwardsVector);
            }
        }
        objectIsMoving = false;
    }





}
