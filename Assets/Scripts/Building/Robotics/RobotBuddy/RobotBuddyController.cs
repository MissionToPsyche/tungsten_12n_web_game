using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotBuddyController : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool FocusedOn;
    [SerializeField, ReadOnly] private float groundCheckRadius = 0.3f;
    [Header("Objects")]
    [SerializeField] private Rigidbody2D RoboBody;
    [SerializeField] private GravityBody2D gravityBody;

    private float walkingSpeed = 5.5f;
    private float idleSpeed = 0f;

    // variables used for ladder movement
    private float vertical;

    [SerializeField, ReadOnly] private float currentSpeed;

    [SerializeField, ReadOnly] private float horizontalInput = 0f;

    private bool isIdle = false;
    private bool isWalking = false;

    private bool isJumping = false;
    private bool isCrouching = false;

    [Header("Movement")]
    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private bool isFacingRight = false;

    private enum PlayerState { Idle, Walking, Interacting }
    [SerializeField, ReadOnly] private PlayerState currentState = PlayerState.Idle;
    [SerializeField] public BoolEvent robotBuddyInteract;
    // Animation
    [SerializeField] private static PlayerController instance;
    private UnityEngine.Vector3 playerCoordinates;
    private GameManager gameManager;
    private bool isInPit;
    [SerializeField, ReadOnly] private bool isActive;
    void Start()
    {
        transform.position = new Vector3(999f, 999f, 0);
    }

    // -------------------------------------------------------------------
    // Handle events
    // this method will set the player's last coordinates on the main asteroid scene
    public void SetPlayerCoordinates()
    {
        this.playerCoordinates = this.transform.position;
    }
    // this method will get the player's last coordinates on the main asteroid scene
    public Vector3 GetPlayerCoordinates()
    {
        return this.playerCoordinates;
    }
    public void OnRobotMove(Vector2 direction)
    {
        if(!isActive)
            return;
        horizontalInput = direction.x;
    }

    public void OnControlStateUpdated(Control.State currentControlState)
    {
        if(currentControlState == Control.State.RobotBuddyAlpha){
            isActive = this.gameObject.name ==  "RobotBuddyAlpha";
        }else if(currentControlState == Control.State.RobotBuddyBeta){
            isActive = this.gameObject.name ==  "RobotBuddyBeta";
        }
    }
    public void OnRobotBuddyInteract(bool interacting)
    {
        if(!isActive)
            return;
        //Fix Module, look how to call repairModule
    }
    private void RobotMove()
    {
        currentSpeed = walkingSpeed;

        // Calculate the movement direction based on the player's current orientation and input
        UnityEngine.Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        UnityEngine.Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);
        // Move the player's rigidbody
        RoboBody.position += movement;
    }

    private void Interact()
    {
        if (isGrounded && currentState == PlayerState.Interacting)
        {
            robotBuddyInteract.Raise(true);
        }
    }

    // User input, animations, moving non-physics objects, game logic


    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        if(!isActive)
            return;
        // First check to make sure the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle possible inputs
        RobotMove();
        Interact();

        // Handle falling in the pit scenario
        if (isInPit)
        {
            gameManager.GetComponent<PlayerManager>().SetScenePosition(SceneManager.GetActiveScene().name);
        }

    }

    // when in contact with objects
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "BlackPit":
                isInPit = true;
            break;

            default:
            break;
        }
    }

    // when not in contact with objects
    private void OnTriggerExit2D(Collider2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "BlackPit":
                isInPit = false;
            break;
            default:
            break;
        }

    }
}
