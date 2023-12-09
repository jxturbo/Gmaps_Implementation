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
        //here to trigger object stasis
        if(Input.GetKeyDown(KeyCode.X) && !objectIsMoving)
        {
            stasisActive = !stasisActive;
            //this specific code is to reset all the objects being affected by the implemented gravity stasis
            //and allow it to function normally under regular gravity(custom and not the rigidbody's gravity)
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
            //checks for anything but the player
            int noPlayerLayerMask = ~(1 << LayerMask.NameToLayer("Player"));
            //this effectively means if player 'selected' a gameobject when a ray from player forward(using mouse position centered for fixed direction), 
            //it will 'float' in the air and follow the player
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, noPlayerLayerMask) && hit.collider.CompareTag("Interactable"))
            {
                Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                ObjectController currentObjectController = hit.collider.gameObject.GetComponent<ObjectController>();
                currentObjectController.noGravity = true;
                hit.collider.gameObject.tag = "Interacting";
                hit.collider.gameObject.transform.SetParent(this.gameObject.transform);
                //if object was falling, reset the velocity
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
                //makes sure that object is upright when it lands
                Vector3 upwardsVector = hit.normal;
                //effectively 'fires' all the current objects in stasis(parent under player) in the direction player points(if the ray hits something)
                GravityStasisObjectAll(hit.point, upwardsVector);
                // Log the name of the object that was hit
                Debug.Log(hit.collider.gameObject.name);
            }          
        }       
    }

    //more or less here to make sure all objects in stasis moves by running their methods one by one
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
                //unparent from player to prevent player movement from affecting it
                obj.transform.SetParent(orignalObjectParent);
                objectController.noGravity = false;
                objectController.StartGravityStasisObject(targetPosition, upwardsVector);
            }
        }
        objectIsMoving = false;
    }





}
