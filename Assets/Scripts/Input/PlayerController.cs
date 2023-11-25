using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField, ReadOnly] private float groundCheckRadius = 1f;

    // Objects
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private GravityBody2D gravityBody;

    // Movement
    [SerializeField] private InputReader input;
    [SerializeField, ReadOnly] private float walkingSpeed = 10f;
    [SerializeField, ReadOnly] private float crouchingSpeed = 5f;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private float jumpSpeed = 4f;
    [SerializeField, ReadOnly] private float moveDirection;
    [SerializeField, ReadOnly] private bool isMoving = false;
    [SerializeField, ReadOnly] private bool isFacingRight = false;
    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private bool isJumping = false;
    [SerializeField, ReadOnly] private bool canDoubleJump = false;
    [SerializeField, ReadOnly] private bool isCrouching = false;
    [SerializeField, ReadOnly] private bool isFalling = false;

    // Interaction
    [SerializeField, ReadOnly] private bool isInteracting = false;

    // Animation
    [SerializeField] private Animator animator;
    private CharacterDatabase characterDatabase;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, ReadOnly] private int selection = 0;

    private float groundedRotationSpeed = 90f;
    private float airborneRotationSpeed = 2f;

    private void Start()
    {
        // Set up event handlers
        input.MoveEvent += HandleMove;
        input.JumpEvent += HandleJump;
        input.JumpCancelledEvent += HandleJumpCancelled;
        input.CrouchEvent += HandleCrouch;
        input.CrouchCancelledEvent += HandleCrouchCancelled;
        input.InteractEvent += HandleInteract;
        input.InteractCancelledEvent += HandleInteractCancelled;

        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selection = 0;
            animator.SetBool("John", true);
        }
        else
        {
            GetSelectedCharacter();
            switch (selection)
            {
                case 0:
                    animator.SetBool("John", true);
                    break;

                case 1:
                    animator.SetBool("Joy", true);
                    break;
            }
        }

        LoadSelectedCharacter(selection);
    }

    // -------------------------------------------------------------------
    // Event handlers

    private void HandleMove(float direction)
    {
        if (isInteracting) return;

        moveDirection = direction;
        isMoving = moveDirection != 0;
        // isFacingRight = moveDirection > 0;
    }

    private void HandleJump()
    {
        if (isCrouching) return;

        isJumping = true;
    }

    private void HandleJumpCancelled()
    {
        isJumping = false;
    }

    private void HandleCrouch()
    {
        if (!isGrounded) return;

        isCrouching = true;
    }

    private void HandleCrouchCancelled()
    {
        isCrouching = false;
    }

    private void HandleInteract()
    {
        if (isMoving || isCrouching || !isGrounded) return;

        isInteracting = true;
    }

    private void HandleInteractCancelled()
    {
        if (!isGrounded) return;

        isInteracting = false;
    }

    // -------------------------------------------------------------------

    private void Move()
    {
        currentSpeed = isCrouching ? crouchingSpeed : walkingSpeed;
        playerBody.velocity = new Vector2(moveDirection * currentSpeed, playerBody.velocity.y);

        // Vector2 direction = transform.right * moveDirection;
        // playerBody.MovePosition(playerBody.position + direction * (currentSpeed * Time.fixedDeltaTime));
    }


    private void Jump()
    {
        if (!isGrounded) return;

        if (isJumping)
        {
            Vector2 jumpDirection = transform.right * moveDirection + transform.up;
            playerBody.AddForce(jumpDirection * jumpSpeed, ForceMode2D.Impulse);
        }

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
        if (isGrounded)
        {
            // do something
        }
    }

    private void Interact()
    {
        if (isGrounded)
        {
            // do something
        }
    }

    // User input, animations, moving non-physics objects, game logic
    private void Update()
    {
        UpdateAnimations();
    }

    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        // First check to make sure the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle possible inputs
        Move();
        Jump();
        Crouch();
        Interact();

        // Rotation logic based on gravity direction
        float targetAngle = Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;
        float currentRotationSpeed = isGrounded ? groundedRotationSpeed : airborneRotationSpeed;
        float smoothedAngle = Mathf.LerpAngle(playerBody.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);

        playerBody.rotation = smoothedAngle;
    }

    private void UpdateAnimations()
    {
        if (!isFacingRight && moveDirection > 0f)
        {
            Flip();
        }
        else if (isFacingRight && moveDirection < 0f)
        {
            Flip();
        }
        
        animator.SetBool("isFacingRight", isFacingRight);
        animator.SetFloat("Horizontal", moveDirection);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isInteracting", isInteracting);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void LoadSelectedCharacter(int selection)
    {
        Character character = characterDatabase.GetSelectedCharacter(selection);
        spriteRenderer.sprite = character.characterSprite;
    }

    private void GetSelectedCharacter()
    {
        selection = PlayerPrefs.GetInt("selectedOption");
    }
}