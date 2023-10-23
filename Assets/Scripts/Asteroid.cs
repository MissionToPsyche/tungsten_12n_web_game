using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // public Transform character;
    // public Transform spaceship;

    // public float rotationSpeed = 1.0f; // default value
    // public float characterRotationMultiplier = 20.0f; // default value
    // public float spaceshipRotationMultiplier = 40.0f;
    // public Transform spaceBackground; // space background
    // public Transform resourceOverlay;
    // public float backgroundRotationFactor = 0.5f; // half the speed of the planet
    // private bool characterOnAsteroid = false;

    // // Gravity management variables
    // public List<Transform> affectedEntities = new List<Transform>(); // List of all entities affected by the asteroid's gravity

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Character"))
    //     {
    //         characterOnAsteroid = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Character"))
    //     {
    //         characterOnAsteroid = false;
    //     }
    // }

    // void Update()
    // {
    //     if (!characterOnAsteroid)
    //     {
    //         // todo 
    //     }
    //     float multiplier = 1.0f; // default value
    //     if (ContextEngine.Instance.currentControlState == ContextEngine.ControlState.Character) 
    //     {
    //         multiplier = characterRotationMultiplier;
    //     }
    //     else if (ContextEngine.Instance.currentControlState == ContextEngine.ControlState.Spaceship) 
    //     {
    //         multiplier = spaceshipRotationMultiplier;
    //     }

    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     // horizontalInput *= GetDirectionMultiplier(); // Acount for camera translations
    //     transform.Rotate(0, 0, horizontalInput * rotationSpeed * multiplier * Time.deltaTime);

    //     //Rotate resource with planet
    //     resourceOverlay.Rotate(0, 0, horizontalInput * rotationSpeed * multiplier * Time.deltaTime);

    //     // Rotate the background with parallax effect
    //     spaceBackground.Rotate(0, 0, horizontalInput * rotationSpeed * backgroundRotationFactor * multiplier * Time.deltaTime);

    //     AdjustEntitiesOrientation(); // Adjust the orientation of entities to align with gravity
    // }

    // void AdjustEntitiesOrientation()
    // {
    //     foreach (Transform entity in affectedEntities)
    //     {
    //         Vector3 directionToCenter = (entity.position - transform.position).normalized;
    //         entity.up = directionToCenter;
    //     }
    // }

    // public void RegisterEntity(Transform entity)
    // {
    //     if (!affectedEntities.Contains(entity))
    //     {
    //         affectedEntities.Add(entity);
    //     }
    // }

    // public void UnregisterEntity(Transform entity)
    // {
    //     if (affectedEntities.Contains(entity))
    //     {
    //         affectedEntities.Remove(entity);
    //     }
    // }
}