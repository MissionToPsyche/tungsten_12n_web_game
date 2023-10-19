using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float verticalSpeed = 5.0f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX; // Lock the X position
    }

    void Update()
    {
        if (ContextEngine.Instance == null)
        {
            Debug.LogError("ContextEngine reference is missing in Spaceship!");
            return;
        }
        
        // Disable spaceship inputs when not in focus
        if (ContextEngine.Instance.currentControlState != ContextEngine.ControlState.Spaceship)
            return;

        // Read player input for vertical movement only
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(0, verticalInput * verticalSpeed * Time.deltaTime, 0);

        // Apply the movement to the spaceship's position
        transform.Translate(movement);
    }
}
