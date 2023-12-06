using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public bool isGrounded = false;
    void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag("Ground"))
        {
            // Change the layer of the collided object to "Selected"
            //collision.gameObject.layer = LayerMask.NameToLayer("Selected");
            isGrounded = true;
        }
    }



}
