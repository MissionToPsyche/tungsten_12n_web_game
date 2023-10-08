using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsycheAsteroid_script : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
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
            transform.Rotate(0, 0, -horizontalInput * rotationSpeed * Time.deltaTime);
        }
    }
}