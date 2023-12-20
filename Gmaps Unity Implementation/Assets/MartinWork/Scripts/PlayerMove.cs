using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 50.0f;
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
        // Ensures my PLAYER stays upright
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 

        hInput = Input.GetAxis("Horizontal"); // A & D keys
        vInput = Input.GetAxis("Vertical"); // W & S keys

        transform.Translate(Vector3.forward * Time.deltaTime * speed * vInput);

        // Rotate the PLAYER around
        transform.Rotate(transform.up, hInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        } 
    }

    private void Jump()
    {
        // Reset velocity 
        rb.velocity = Vector3.zero;

        // Set the jump direction to be PLAYER's 'UP'
        Vector3 jumpDir = transform.up; 

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
