using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Public variables for editor access.
    public CinemachineVirtualCamera playerCamera;

    // Exposed for editing in the editor but not intended for external access.
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;

    // Private variables for internal state and component caching.
    private bool isGrounded => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    private Rigidbody2D objectBody2D;
    private Vector2 playerInputValue;
    private GravityBody2D gravityBody;
    private bool isControllerActive = false;
    private bool rotateToWorldUp = false;
    private float groundedRotationSpeed = 90f;
    private float airborneRotationSpeed = 2f;
    private float groundCheckRadius = 0.3f;
    private float speed = 15;
    private float jumpForce = 20f;
    private float cameraRotationSpeed = 2.0f;

    private void Start()
    {
        ContextEngine.Instance.OnContextChanged += HandleContextChanged;

        objectBody2D = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();

        gravityBody.OnEnterGravityArea += HandleEnterGravityArea;
        gravityBody.OnExitGravityArea += HandleExitGravityArea;
    }

    private void OnDestroy()
    {
        ContextEngine.Instance.OnContextChanged -= HandleContextChanged;
    }

    public void EnableController()
    {
        isControllerActive = true;
        playerCamera.Priority = 100;
    }

    public void DisableController()
    {
        isControllerActive = false;
        playerCamera.Priority = 0;
        objectBody2D.velocity = Vector2.zero; // Stop the player's movement
    }
    private void HandleContextChanged(ContextEngine.ControlState newState)
    {
        // Implementation for context change if necessary.
    }

    private void UpdateCameraOrientation()
    {
        // Implement updating the player's camera orientation if needed.
    }

    private void OnMove(InputValue value)
    {
        if (!isControllerActive) return;
        playerInputValue = value.Get<Vector2>();
    }

    private void EnteredNewGravityOrbit(Transform newAsteroid)
    {
        ContextEngine.Instance.asteroid = newAsteroid;
    }

    public void OnJump()
    {
        if (!isControllerActive || !isGrounded) return;

        Vector2 jumpDirection = playerInputValue.x != 0 ? 
            (-gravityBody.GravityDirection + new Vector2(playerInputValue.x, 0)).normalized : 
            -gravityBody.GravityDirection;

        objectBody2D.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleEnterGravityArea(GravityArea2D gravityArea)
    { 
        rotateToWorldUp = false;
    }

    private void HandleExitGravityArea(GravityArea2D gravityArea)
    {
        rotateToWorldUp = true;
    }

    private void Update()
    {
        // Camera rotation logic can be placed here if necessary.
        if (isControllerActive)
        {
            Quaternion currentRotation = playerCamera.transform.rotation;
            Quaternion targetRotation = transform.rotation;
            
            playerCamera.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded && playerInputValue.x != 0)
        {
            Vector2 direction = transform.right * playerInputValue.x;
            objectBody2D.MovePosition(objectBody2D.position + direction * (speed * Time.fixedDeltaTime));
        }

        float targetAngle = rotateToWorldUp ? 
            0f : 
            Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;

        float currentRotationSpeed = isGrounded ? groundedRotationSpeed : airborneRotationSpeed;
        float smoothedAngle = Mathf.LerpAngle(objectBody2D.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);

        objectBody2D.rotation = smoothedAngle;
    }
}
