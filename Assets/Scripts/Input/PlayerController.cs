using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField, ReadOnly] private float groundCheckRadius = 0.3f;

    // Objects
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private GravityBody2D gravityBody;

    // Movement
    private float sprintingSpeed = 10f;
    private float walkingSpeed = 7.5f;
    private float strafingSpeed = 5f;
    private float crawlingSpeed = 2.5f;
    private float idleSpeed = 0f;
    [SerializeField, ReadOnly] private float currentSpeed;

    [SerializeField] private float jumpForce = 1f;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    // [SerializeField, ReadOnly] private float verticalInput = 0f;

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

    private void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selection = 0;
            animator.SetBool("John", true);
        }
        else
        {
            selection = PlayerPrefs.GetInt("selectedOption");
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

    private void OnEnable()
    {
        // Subscribe to events
        inputReader.PlayerMove += OnPlayerMove;
        inputReader.PlayerSprint += OnPlayerSprint;
        inputReader.PlayerSprintCancelled += OnPlayerSprintCancelled;
        inputReader.PlayerJump += OnPlayerJump;
        inputReader.PlayerJumpCancelled += OnPlayerJumpCancelled;
        inputReader.PlayerCrouch += OnPlayerCrouch;
        inputReader.PlayerCrouchCancelled += OnPlayerCrouchCancelled;
        inputReader.PlayerInteract += OnPlayerInteract;
        inputReader.PlayerInteractCancelled += OnPlayerInteractCancelled;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        inputReader.PlayerMove -= OnPlayerMove;
        inputReader.PlayerSprint -= OnPlayerSprint;
        inputReader.PlayerSprintCancelled -= OnPlayerSprintCancelled;
        inputReader.PlayerJump -= OnPlayerJump;
        inputReader.PlayerJumpCancelled -= OnPlayerJumpCancelled;
        inputReader.PlayerCrouch -= OnPlayerCrouch;
        inputReader.PlayerCrouchCancelled -= OnPlayerCrouchCancelled;
        inputReader.PlayerInteract -= OnPlayerInteract;
        inputReader.PlayerInteractCancelled -= OnPlayerInteractCancelled;
    }

    // -------------------------------------------------------------------
    // Handle events

    private void OnPlayerMove(Vector2 direction)
    {
        horizontalInput = direction.x;
        isIdle = horizontalInput == 0;

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

    private void OnPlayerSprint()
    {
        canSprint = true;

        if (isGrounded && !isCrouching && !isIdle)
        {
            isSprinting = true;
            UpdatePlayerState(PlayerState.Sprinting);
        }
    }

    private void OnPlayerSprintCancelled()
    {
        canSprint = false;
        isSprinting = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }

    private void OnPlayerJump()
    {
        if (isGrounded)
        {
            isJumping = true;
            UpdatePlayerState(PlayerState.Jumping);
        }
    }

    private void OnPlayerJumpCancelled()
    {
        isJumping = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }

    private void OnPlayerCrouch()
    {
        if (isGrounded)
        {
            isCrouching = true;
            UpdatePlayerState(isIdle ? PlayerState.Crouching : PlayerState.Crawling);
        }
    }

    private void OnPlayerCrouchCancelled()
    {
        isCrouching = false;
        UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
    }


    private void OnPlayerInteract()
    {
        if (currentState != PlayerState.Idle) return;

        UpdatePlayerState(PlayerState.Interacting);
    }

    private void OnPlayerInteractCancelled()
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

        // Calculate the movement direction based on the player's current orientation and input
        Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);
        // Move the player's rigidbody
        playerBody.position += movement;
    }

    private void Jump()
    {
        if (!isGrounded) return;

        if (currentState == PlayerState.Jumping)
        {
            playerBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
        }
        else if (canDoubleJump)
        {
            // // Double jump
            // playerBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
            // animator.SetTrigger("Jump-Press");
            // canDoubleJump = false;
        }
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
    }

    private void UpdateAnimations()
    {
        if (!isFacingRight && horizontalInput > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontalInput < 0f)
        {
            Flip();
        }

        animator.SetBool("isFacingRight", isFacingRight);
        animator.SetFloat("Horizontal", horizontalInput);
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
}