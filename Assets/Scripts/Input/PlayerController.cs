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
    private float sprintingSpeed = 10f;
    private float walkingSpeed = 7.5f;
    private float strafingSpeed = 5f;
    private float crawlingSpeed = 2.5f;
    private float idleSpeed = 0f;
    [SerializeField, ReadOnly] private float currentSpeed;

    [SerializeField, ReadOnly] private float jumpForce = 4f;
    [SerializeField, ReadOnly] private float moveDirection = 0f;

    private bool isIdle = false;
    private bool isSprinting = false;
    private bool isWalking = false;
    private bool isCrawling = false;
    private bool isStrafing = false;

    private bool isJumping = false;
    private bool isCrouching = false;

    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private bool canSprint = false;
    [SerializeField, ReadOnly] private bool canDoubleJump = false;
    [SerializeField, ReadOnly] private bool isFacingRight = false;

    private enum PlayerState { Idle, Walking, Sprinting, Strafing, Jumping, Crouching, Crawling, Interacting }
    [SerializeField, ReadOnly] private PlayerState currentState = PlayerState.Idle;

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
        input.SprintEvent += HandleSprint;
        input.SprintCancelledEvent += HandleSprintCancelled;
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
        moveDirection = direction;
        isIdle = direction == 0;

        if (isIdle)
        {
            UpdatePlayerState(isCrouching ? PlayerState.Crouching : PlayerState.Idle);
        }
        else if (isSprinting)
        {
            UpdatePlayerState(PlayerState.Sprinting);
        }
        else if (isCrouching)
        {
            UpdatePlayerState(PlayerState.Crawling);
        }
        else if (!isGrounded)
        {
            UpdatePlayerState(PlayerState.Strafing);
        }
        else
        {
            UpdatePlayerState(PlayerState.Walking);
        }
    }

    private void HandleSprint()
    {
        canSprint = true;

        if (isGrounded && !isCrouching && !isIdle)
        {
            isSprinting = true;
            UpdatePlayerState(PlayerState.Sprinting);
        }
    }

    private void HandleSprintCancelled()
    {
        canSprint = false;
        isSprinting = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            isJumping = true;
            UpdatePlayerState(PlayerState.Jumping);
        }
    }

    private void HandleJumpCancelled()
    {
        isJumping = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }

    private void HandleCrouch()
    {
        if (isGrounded)
        {
            isCrouching = true;
            UpdatePlayerState(isIdle ? PlayerState.Crouching : PlayerState.Crawling);
        }
    }

    private void HandleCrouchCancelled()
    {
        isCrouching = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }


    private void HandleInteract()
    {
        if (currentState != PlayerState.Idle) return;

        UpdatePlayerState(PlayerState.Interacting);
    }

    private void HandleInteractCancelled()
    {
        if (currentState == PlayerState.Interacting)
        {
            UpdatePlayerState(PlayerState.Idle);
        }
    }
    // -------------------------------------------------------------------

    private void UpdatePlayerState(PlayerState newState)
    {
        currentState = newState;
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        // Reset all animator parameters
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("canSprint", false);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isCrawling", false);
        animator.SetBool("isStrafing", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isInteracting", false);

        animator.SetBool("canSprint", canSprint);

        // Update based on current state
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case PlayerState.Walking:
                animator.SetBool("isWalking", true);
                break;
            case PlayerState.Sprinting:
                animator.SetBool("isSprinting", true);
                break;
            case PlayerState.Crouching:
                animator.SetBool("isIdle", true);
                animator.SetBool("isCrouching", true);
                break;
            case PlayerState.Crawling:
                animator.SetBool("isCrawling", true);
                break;
            case PlayerState.Strafing:
                animator.SetBool("isStrafing", true);
                break;
            case PlayerState.Jumping:
                animator.SetBool("isJumping", true);
                break;
            case PlayerState.Interacting:
                animator.SetBool("isIdle", true);
                animator.SetBool("isInteracting", true);
                break;
        }
    }

    private void Move()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                currentSpeed = idleSpeed;
                break;
            case PlayerState.Crawling:
                currentSpeed = crawlingSpeed;
                break;
            case PlayerState.Strafing:
                currentSpeed = strafingSpeed;
                break;
            case PlayerState.Walking:
                currentSpeed = walkingSpeed;
                break;
            case PlayerState.Sprinting:
                currentSpeed = sprintingSpeed;
                break;
        }
        
        playerBody.velocity = new Vector2(moveDirection * currentSpeed, playerBody.velocity.y);

        // Vector2 direction = transform.right * moveDirection;
        // playerBody.MovePosition(playerBody.position + direction * (currentSpeed * Time.fixedDeltaTime));
    }


    private void Jump()
    {
        if (!isGrounded) return;

        if (currentState == PlayerState.Jumping)
        {
            Vector2 jumpDirection = transform.right * moveDirection + transform.up;
            playerBody.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
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
        animator.SetBool("isGrounded", isGrounded);
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