using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_script : MonoBehaviour
{
    public float rotationSpeed = 20.0f;
    public Transform spaceBackground; // space background
    public float backgroundRotationFactor = 0.5f; // half the speed of the planet
    private bool characterOnAsteroid = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            characterOnAsteroid = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            characterOnAsteroid = false;
        }
    }

    void Update()
    {
        if (!characterOnAsteroid)
        {
            // Rotate the asteroid based on player input
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(0, 0, horizontalInput * rotationSpeed * Time.deltaTime);

            // Rotate the background with parallax effect
            spaceBackground.Rotate(0, 0, horizontalInput * rotationSpeed * backgroundRotationFactor * Time.deltaTime);
        }
    }
}