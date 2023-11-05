using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMover : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 90.0f; // Rotation speed in degrees per second

    private bool isMoving = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isMoving)
        {
            float horizontalInput = 0.0f;
            float verticalInput = 0.0f;

            // Check for IJKL key input (movement)
            if (Input.GetKey("i"))
                verticalInput = 1.0f;
            if (Input.GetKey("k"))
                verticalInput = -1.0f;
            if (Input.GetKey("j"))
                horizontalInput = -1.0f;
            if (Input.GetKey("l"))
                horizontalInput = 1.0f;

            // Check for U and O key input (rotation)
            if (Input.GetKey("u"))
                RotateEntity(-1.0f); // Rotate left
            if (Input.GetKey("o"))
                RotateEntity(1.0f); // Rotate right

            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0.0f);

            // Normalize the direction to prevent faster diagonal movement
            if (moveDirection.magnitude > 1.0f)
            {
                moveDirection.Normalize();
            }

            // Move the entity
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    void RotateEntity(float direction)
    {
        // Rotate the entity based on the provided direction
        transform.Rotate(Vector3.forward, direction * rotateSpeed * Time.deltaTime);
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Set the entity's velocity to zero to make it stop upon collision
            rb.velocity = Vector2.zero;
        }
    }
}
