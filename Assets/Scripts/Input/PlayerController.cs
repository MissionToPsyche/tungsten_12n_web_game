using UnityEngine;
using Cinemachine;
using System;
using System.Numerics;
using UnityEngine.SceneManagement;
using UnityEditor.Callbacks;
using TMPro;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField, ReadOnly] private float groundCheckRadius = 0.3f;
    [Header("Objects")]
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private GravityBody2D gravityBody;

    private float sprintingSpeed = 10f;
    private float walkingSpeed = 7.5f;
    private float strafingSpeed = 5f;
    private float crawlingSpeed = 2.5f;
    private float idleSpeed = 0f;

    // variables used for ladder movement
    private float vertical;
    private bool isLadder;
    private bool isClimibing;
    private bool isInPit;

    [SerializeField, ReadOnly] private float currentSpeed;

    [SerializeField] private float jumpForce = 0f;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    // [SerializeField, ReadOnly] private float verticalInput = 0f;

    private bool isIdle = false;
    private bool isSprinting = false;
    private bool isWalking = false;
    private bool isCrawling = false;
    private bool isStrafing = false;

    private bool isJumping = false;
    private bool isCrouching = false;

    [Header("Movement")]
    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private bool canSprint = false;
    [SerializeField, ReadOnly] private bool canDoubleJump = false;
    [SerializeField, ReadOnly] private bool isFacingRight = false;

    private enum PlayerState { Idle, Walking, Sprinting, Strafing, Jumping, Crouching, Crawling, Interacting }
    [SerializeField, ReadOnly] private PlayerState currentState = PlayerState.Idle;
    [SerializeField] public BoolEvent playerInteract;
    [SerializeField] public BoolEvent playerLaunchPadInteract;
    // Animation
    [SerializeField] private Animator animator;
    private CharacterDatabase characterDatabase;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, ReadOnly] private int selection = 0;
    [SerializeField] private static PlayerController instance;
    private UnityEngine.Vector3 playerCoordinates;
    private GameManager gameManager;
    private TextMeshProUGUI reminderText;


    void Start()
    {
        inputReader.SetRobotBuddyAlpha(false);
        inputReader.SetRobotBuddyBeta(false);
        // // Singleton method
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

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
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        LoadSelectedCharacter(selection);
    }

    // -------------------------------------------------------------------

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // -------------------------------------------------------------------
    // Handle events
    // this method will set the player's last coordinates on the main asteroid scene
    public void SetPlayerCoordinates() 
    {
        this.playerCoordinates = this.transform.position;
    }
    // this method will get the player's last coordinates on the main asteroid scene
    public UnityEngine.Vector3 GetPlayerCoordinates() 
    {
        return this.playerCoordinates;
    }


    public void OnPlayerMove(UnityEngine.Vector2 direction)
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

    public void OnPlayerSprint(bool sprinting)
    {
        // Debug.Log($"Player controller - OnPlayerSprint: {sprinting}");
        if (sprinting)
        {
            canSprint = true;
            if (isGrounded && !isCrouching && !isIdle)
            {
                isSprinting = true;
                UpdatePlayerState(PlayerState.Sprinting);
                // Additional logic for starting sprint
            }
        }
        else
        {
            // Handle sprint cancellation
            canSprint = false;
            isSprinting = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
            // Additional logic for cancelling sprint
        }
    }

    public void OnPlayerJump(bool jumping)
    {
        // Debug.Log($"Player controller - OnPlayerJump: {jumping}");
        if (jumping)
        {
            if (isGrounded)
            {
                isJumping = true;
                UpdatePlayerState(PlayerState.Jumping);
            }
        }
        else
        {
            isJumping = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);

        }
    }

    public void OnPlayerCrouch(bool crouching)
    {
        if (crouching)
        {
            if (isGrounded)
            {
                isCrouching = true;
                UpdatePlayerState(isIdle ? PlayerState.Crouching : PlayerState.Crawling);
            }
        }
        else
        {
            isCrouching = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
        }
    }

    public void OnPlayerInteract(bool interacting)
    {
        if (interacting)
        {
            if (currentState != PlayerState.Idle) return;
            UpdatePlayerState(PlayerState.Interacting);
        }
        else
        {
            if (currentState == PlayerState.Interacting)
            {
                UpdatePlayerState(PlayerState.Idle);
            }
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
        UnityEngine.Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        UnityEngine.Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);
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
        if (isGrounded && currentState == PlayerState.Interacting)
        {
            playerInteract.Raise(true);
            //playerLaunchPadInteract.Raise(true);
        }
    }

    // User input, animations, moving non-physics objects, game logic
    private void Update()
    {
        UpdateAnimations();

        // below is the code for climbing ladders
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimibing = true;
        }
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

        // Handle falling in the pit scenario
        if (isInPit)
        {
            gameManager.GetComponent<PlayerManager>().setScenePosition(SceneManager.GetActiveScene().name);
        }

        // Handle ladder movement
        if (isClimibing && Input.GetKey(KeyCode.W))
        {
            this.playerBody.velocity = new UnityEngine.Vector2(playerBody.velocity.x, 4f);
        }
        else if (isClimibing && Input.GetKey(KeyCode.S))
        {
            this.playerBody.velocity = new UnityEngine.Vector2(playerBody.velocity.x, -4f);
        }
    }

    public void UpdateJumpForce(float newJumpForce)
    {
        this.jumpForce = newJumpForce;
    }

    public float GetJumpForce()
    {
        return this.jumpForce;
    }

    // when in contact with objects
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "Ladder":
                isLadder = true;
                break;

            case "BlackPit":
                isInPit = true;
            break;

            default:
            break;
        }
        // if (Collision.gameObject.tag == "Ladder")
        // {
        //     isLadder = true;
        // }
    }

    // when not in contact with objects
    private void OnTriggerExit2D(Collider2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "Ladder":
                isLadder = false;
                isClimibing = false;
                break;

            case "BlackPit":
                isInPit = false;
            break;

            default:
            break;
        }

        // if (Collision.gameObject.tag == "Ladder")
        // {
        //     isLadder = false;
        //     isClimibing = false;
        // }
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

        UnityEngine.Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void LoadSelectedCharacter(int selection)
    {
        Character character = characterDatabase.GetSelectedCharacter(selection);
        spriteRenderer.sprite = character.characterSprite;
    }

    public void OnBuildRobotBuddyObject(BuildingComponents.BuildingType buildingType){
        Debug.Log("In BuildingRobotBuddy playerController");
        if(buildingType == BuildingComponents.BuildingType.RobotBuddyAlpha){
            inputReader.SetRobotBuddyAlpha(true);
        }else if (buildingType == BuildingComponents.BuildingType.RobotBuddyBeta){
            inputReader.SetRobotBuddyBeta(true);
        }
    }
}
