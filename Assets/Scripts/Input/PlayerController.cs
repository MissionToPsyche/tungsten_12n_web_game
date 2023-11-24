using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private float groundCheckRadius = 1f;

    //--------------------------------------------------
    // Objects
    private Rigidbody2D playerBody;
    private GravityBody2D gravityBody;

    //--------------------------------------------------
    // Movement
    public GameInput playerControls;
    private Vector3 moveDirection;
    private bool isJumping;
    private bool isCrouching;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    private bool isFacingRight = false;
    private bool canDoubleJump = true;

    //--------------------------------------------------
    // Animation
    private CharacterDatabase characterDB;
    private SpriteRenderer artworkSprite; 
    private int selectedOption = 0; 


    private bool isControllerActive = false;
    private float groundedRotationSpeed = 90f;
    private float airborneRotationSpeed = 2f;

    private float cameraRotationSpeed = 2.0f;

    private void Start()
    {
        input.MoveEvent += HandleMove;
        input.JumpEvent += HandleJump;
        input.JumpCancelledEvent += HandleJumpCancelled;
        input.CrouchEvent+= HandleCrouch;
        input.CrouchCancelledEvent += HandleCrouchCancelled;

        ContextEngine.Instance.OnContextChanged += HandleContextChanged;

        playerBody = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();

        Debug.Log("Selected option is: " + selectedOption);

        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
            animator.SetBool("John", true);
        }
        else
        {
            Load();
            switch (selectedOption)
            {
                case 0:
                    animator.SetBool("John", true);
                    break;

                case 1:
                    animator.SetBool("Joy", true);
                    break;
            }
        }

        UpdateCharacter(selectedOption);
    }

    private void Update()
    {
        Move();
        Jump();
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        if (!isControllerActive) return;

        if (!isFacingRight && moveDirection.x > 0f)
        {
            Flip();
        }
        else if (isFacingRight && moveDirection.x < 0f)
        {
            Flip();
        }

        animator.SetFloat("Direction", isFacingRight ? 1 : -1);
        animator.SetFloat("Horizontal", moveDirection.x);
        // animator.SetFloat("Vertical", moveDirection.y);
        animator.SetFloat("Magnitude", moveDirection.magnitude);
        animator.SetBool("Jump-Press", isJumping);
        animator.SetBool("Crouch-Hold", isCrouching);
    }

    private void HandleMove(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void HandleJump()
    {
        isJumping = true;
        // animator.SetTrigger("Jump-Press");
        canDoubleJump = true;
    }

    private void HandleJumpCancelled()
    {
        isJumping = false;
    }

    private void HandleCrouch()
    {
        isCrouching = true;
    }

    private void HandleCrouchCancelled()
    {
        isCrouching = false;
    }

    private void Move()
    {
        if (!isControllerActive) return;

        if (moveDirection == Vector3.zero) return;

        if (IsGrounded())
        {
            Vector2 direction = transform.right * moveDirection.x;
            playerBody.MovePosition(playerBody.position + direction * (movementSpeed * Time.fixedDeltaTime));
        }
    }

    private void Jump()
    {
        if (!isControllerActive || !IsGrounded()) return;

        // if (isJumping)
        // {
        //     transform.position += new Vector3(0, 1, 0) * (jumpSpeed * Time.deltaTime);
        // }
        if (isJumping)
        {
            Vector2 jumpDirection = transform.right * moveDirection.x + transform.up;
            playerBody.AddForce(jumpDirection * jumpSpeed, ForceMode2D.Impulse);
        }

            // playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
        // else if (canDoubleJump)
        // {
        //     // Double jump
        //     Vector2 jumpDirection = -gravityBody.GravityDirection;
        //     playerBody.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        //     animator.SetTrigger("Jump-Press");
        //     canDoubleJump = false;
        // }
    }

    private void Crouch()
    {

    }

    private void Interact()
    {
        if (!isControllerActive || !IsGrounded()) return;

        animator.SetBool("Interaction", true);
    }

    private void SwitchContext()
    {

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
        // Implementation for context change if necessary.
    }

    private void UpdateCameraOrientation()
    {
        // Implement updating the player's camera orientation if needed.
    }

    private void EnteredNewGravityOrbit(Transform newAsteroid)
    {
        ContextEngine.Instance.asteroid = newAsteroid;
    }

    private void FixedUpdate()
    {
        // playerBody.MovePosition(playerBody.position + (Vector2)transform.TransformDirection(moveDirection) * movementSpeed * Time.deltaTime);
        // Rotation logic based on gravity direction
        float targetAngle = Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;

        float currentRotationSpeed = IsGrounded() ? groundedRotationSpeed : airborneRotationSpeed;
        float smoothedAngle = Mathf.LerpAngle(playerBody.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);

        playerBody.rotation = smoothedAngle;

        if (isControllerActive)
        {
            Quaternion currentRotation = playerCamera.transform.rotation;
            Quaternion targetRotation = transform.rotation;
            
            playerCamera.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateCharacter(int selectedOption) 
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite; 
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}