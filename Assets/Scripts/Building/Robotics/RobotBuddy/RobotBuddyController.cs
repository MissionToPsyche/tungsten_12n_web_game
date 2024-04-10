using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RobotBuddyController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool FocusedOn;
    [SerializeField, ReadOnly] private float groundCheckRadius = 0.3f;
    [Header("Objects")]
    [SerializeField] private Rigidbody2D robotBody;
    [SerializeField] private GravityBody2D gravityBody;

    private float walkingSpeed = 5.5f;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    private bool isIdle = false;
    [SerializeField] private float jumpForce = 1f;

    [Header("Movement")]
    [SerializeField, ReadOnly] private bool isGrounded = false;

    private enum RobotState { Idle, Walking, Interacting , Jumping}
    [SerializeField, ReadOnly] private RobotState currentState = RobotState.Idle;
    [SerializeField] public BoolEvent robotBuddyInteract;
    [SerializeField] private FloatEvent adjustRobotUI;
    // Animation
    private Vector3 playerCoordinates;
    private GameManager gameManager;
    private bool isInPit;
    private bool playerCanInteract = false;
    [SerializeField, ReadOnly] private bool isActive;
    [SerializeField, ReadOnly] private int TechTier = 0;
    RobotBuddy robotBuddy;
    CapsuleCollider2D chargeCollider;
    void Awake()
    {
        transform.position = new Vector3(999f, 999f, 0);
        robotBuddy = new();
        chargeCollider = gameObject.AddComponent<CapsuleCollider2D>();
        chargeCollider.isTrigger = true;
    }

    IEnumerator DecreaseCharge()
    {
        while(isActive)
        {
            //reduces 1 tick per 1 second
            adjustRobotUI.Raise(robotBuddy.ReduceCharge(1.0f));
            yield return new WaitForSeconds(1.0f);
        }
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
            StartCoroutine(DecreaseCharge());
        }else if(currentControlState == Control.State.RobotBuddyBeta){
            isActive = this.gameObject.name ==  "RobotBuddyBeta";
            StartCoroutine(DecreaseCharge());
        }
    }
    public void OnRobotBuddyInteract(bool interacting)
    {
        if(!isActive)
            return;
        //Fix Module, look how to call repairModule
    }
    public void OnRobotJump(bool jumping)
    {
        //Debug.Log($"Player controller - OnPlayerJump: {jumping}");
        adjustRobotUI.Raise(robotBuddy.ReduceCharge(5.0f));
        if (jumping)
        {
            if (isGrounded)
            {
                UpdateRobotState(RobotState.Jumping);
            }
        }
        else
        {
            UpdateRobotState(isIdle ? RobotState.Idle : RobotState.Walking);
        }
    }
    private void RobotMove()
    {
        currentSpeed = walkingSpeed;

        // Calculate the movement direction based on the player's current orientation and input
        UnityEngine.Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        UnityEngine.Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);
        // Move the player's rigidbody
        robotBody.position += movement;
    }

    private void Interact()
    {
        if (isGrounded && currentState == RobotState.Interacting)
        {
            adjustRobotUI.Raise(robotBuddy.ReduceCharge(1.0f));
            robotBuddyInteract.Raise(true);
        }
    }
    private void Jump()
    {
        if (!isGrounded) return;

        if (currentState == RobotState.Jumping)
        {
            robotBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
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
        Jump();
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
            case "Player":
                playerCanInteract = true;
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
            case "Player":
                playerCanInteract = false;
            break;
            default:
            break;
        }
    }
    
    public void OnPlayerInteract(){
        if(playerCanInteract){
            robotBuddy.GiveFullCharge();
            adjustRobotUI.Raise(robotBuddy.GetCurrentCharge());
        }
    }
    private void UpdateRobotState(RobotState newState)
    {
        currentState = newState;
    }
}
