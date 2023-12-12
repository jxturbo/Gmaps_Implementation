using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public bool isOnGround = false;

    private float hInput;
    private float vInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * vInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * hInput);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            // rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // isOnGround = false; 
            // Previous jump use 

            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = Vector3.zero; // Reset velocity 

        Vector3 jumpDir = transform.up; // Set the jump direction to be player's UP

        rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
