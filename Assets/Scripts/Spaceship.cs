using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;

    void Update()
    {
        // Read player input for horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement in both horizontal and vertical directions
        Vector3 movement = new Vector3(horizontalInput * horizontalSpeed * Time.deltaTime,
                                       verticalInput * verticalSpeed * Time.deltaTime,
                                       0);

        // Apply the movement to the spaceship's position
        transform.Translate(movement);
    }
}
