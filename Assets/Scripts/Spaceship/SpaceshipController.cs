using UnityEngine;
using Cinemachine;

public class SpaceshipController : MonoBehaviour
{
    // Public variables for editor access.
    public CinemachineVirtualCamera spaceshipCamera;
    public float verticalSpeed = 5.0f;

    // Private variables for internal state and component caching.
    private Rigidbody2D rb;
    private bool isControllerActive = false;

    private void Start()
    {
        // Cache the Rigidbody2D component for later use.
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Activates the spaceship's controller and prioritizes its camera.
    /// </summary>
    public void EnableController()
    {
        isControllerActive = true;
        spaceshipCamera.Priority = 100;
    }

    /// <summary>
    /// Deactivates the spaceship's controller and deprioritizes its camera.
    /// </summary>
    public void DisableController()
    {
        isControllerActive = false;
        spaceshipCamera.Priority = 0;
    }

    void Update()
    {
        // Ensure that ContextEngine is accessible.
        if (ContextEngine.Instance == null)
        {
            Debug.LogError("ContextEngine reference is missing in Spaceship!");
            return;
        }

        // Exit early if the spaceship isn't the currently active controller.
        if (ContextEngine.Instance.currentControlState != ContextEngine.ControlState.Spaceship)
            return;

        // Read player input for vertical movement and apply to the spaceship's position.
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(0, verticalInput * verticalSpeed * Time.deltaTime, 0);
    }
}
