using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public bool isGrounded = false;
    //This code is mostly there to check if the player is currently touching the ground
    //done on a seperate script to clean it up
    void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }



}
