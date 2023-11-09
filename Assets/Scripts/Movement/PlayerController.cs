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
    private bool isFacingRight = false;
    private float groundedRotationSpeed = 90f;
    private float airborneRotationSpeed = 2f;
    private float groundCheckRadius = 0.5f;
    private float speed = 10;
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
        //Debug.Log("Player OnMove pressed");
        playerInputValue = value.Get<Vector2>();
    }

    private void EnteredNewGravityOrbit(Transform newAsteroid)
    {
        ContextEngine.Instance.asteroid = newAsteroid;
    }

    public void OnJump()
    {
        if (!isControllerActive || !isGrounded) return;
        //Debug.Log("Player OnJump pressed");

        Vector2 jumpDirection = isGrounded ? 
            transform.right * playerInputValue.x + transform.up : 
            -gravityBody.GravityDirection;

        objectBody2D.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump-Press");
    }

    // public void OnCrouch()
    // {
    //     if (!isControllerActive || !isGrounded) return;

    //     //Debug.Log("OnCrouch pressed");

    //     animator.SetBool("Crouch-Hold", true);
    // }

    public void OnInteract()
    {
        if (!isControllerActive || !isGrounded) return;

        //Debug.Log("Player OnInteract pressed");

        animator.SetTrigger("Interaction-Press");
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
        if (isControllerActive)
        {
            Quaternion currentRotation = playerCamera.transform.rotation;
            Quaternion targetRotation = transform.rotation;
            
            playerCamera.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //Debug.Log("Player OnCrouch Pressed");
            animator.SetBool("Crouch-Hold", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch-Hold", false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = playerInputValue;

        if (isGrounded && movement.x != 0)
        {
            Vector2 direction = transform.right * movement.x;
            objectBody2D.MovePosition(objectBody2D.position + direction * (speed * Time.fixedDeltaTime));

            // Flip character based on movement direction
            if (movement.x > 0 && !isFacingRight)
            {
                FlipCharacter();
            }
            else if (movement.x < 0 && isFacingRight)
            {
                FlipCharacter();
            }

            // Update animator values
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
            animator.SetFloat("Direction", isFacingRight ? 1 : -1);
        }
        else 
        {
            animator.SetFloat("Magnitude", 0);
        }

        float targetAngle = rotateToWorldUp ? 
            0f : 
            Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;

        float currentRotationSpeed = isGrounded ? groundedRotationSpeed : airborneRotationSpeed;
        float smoothedAngle = Mathf.LerpAngle(objectBody2D.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);

        objectBody2D.rotation = smoothedAngle;
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}