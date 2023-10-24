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
    private Rigidbody2D playerBody;
    private Vector2 playerInputValue;
    private GravityBody2D gravityBody;

    private bool isControllerActive = false;
    private bool rotateToWorldUp = false;
    private float groundedRotationSpeed = 90f;
    private float airborneRotationSpeed = 2f;
    private float groundCheckRadius = 1f;
    private float speed = 15;
    private float jumpForce = 20f;

    private void Start()
    {
        ContextEngine.Instance.OnContextChanged += HandleContextChanged;

        playerBody = GetComponent<Rigidbody2D>();
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
        playerBody.velocity = Vector2.zero; // Stop the player's movement
    }

    private void HandleContextChanged(ContextEngine.ControlState newState)
    {
        // Implementation for context change if needed
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

        playerBody.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleEnterGravityArea()
    {
        rotateToWorldUp = false;
    }

    private void HandleExitGravityArea()
    {
        rotateToWorldUp = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isGrounded && playerInputValue.x != 0)
        {
            Vector2 direction = transform.right * playerInputValue.x;
            playerBody.MovePosition(playerBody.position + direction * (speed * Time.fixedDeltaTime));
        }

        float targetAngle = rotateToWorldUp ? 
            0f : 
            Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;

        float currentRotationSpeed = isGrounded ? groundedRotationSpeed : airborneRotationSpeed;
        float smoothedAngle = Mathf.LerpAngle(playerBody.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);

        playerBody.rotation = smoothedAngle;
    }
}
