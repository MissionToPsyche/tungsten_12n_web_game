using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//need this library to access the input system features, such as InputValue
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]  private float movementSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private int facingDirection = 1;
    public bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;
    private Rigidbody2D playerBody;
    private CapsuleCollider2D playerCapsule;

    //used to store the player input value, stores and x (left/right) and y (up/down) value
    private Vector2 playerInputValue;
    [SerializeField] private GravityPoint gravityPoint;

    public GameObject asteroid;
    private Vector2 asteroidCenter => asteroid.transform.position;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCapsule = GetComponent<CapsuleCollider2D>();

        capsuleColliderSize = playerCapsule.size;
        gravityPoint = FindObjectOfType<GravityPoint>(); // Assuming there's only one GravityPoint in the scene
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
        AdjustOrientationToGravity();
    }

    private void CheckGround()
    {
        // Vector2 bottom = transform.position - new Vector2(0, height);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(playerBody.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

        private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && playerInputValue.x == 0.0f)
        {
            playerBody.sharedMaterial = fullFriction;
        }
        else
        {
            playerBody.sharedMaterial = noFriction;
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            playerBody.velocity = newVelocity;
            newForce.Set(0.0f, jumpForce);
            playerBody.AddForce(newForce, ForceMode2D.Impulse);
        }
    }  
    //This method is derived from our player input component/input actions
    //We created an action called Move in our Input action asset.
    //The player input component on our player sends meesages via the action names.
    //Example: Move turns to OnMove, if we had an action called Run, it would send a message to OnRun, ect..
    private void OnMove(InputValue value)
    {
        //store player input value in form of vector2. left/right is x values, up/down is y values
        playerInputValue = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    // private void ApplyMovement()
    // {
    //     if (isGrounded && !isOnSlope && !isJumping) //if not on slope
    //     {
    //         newVelocity.Set(movementSpeed * playerInputValue.x, 0.0f);
    //         playerBody.velocity = newVelocity;
    //     }
    //     else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
    //     {
    //         newVelocity.Set(movementSpeed * slopeNormalPerp.x * -playerInputValue.x, movementSpeed * slopeNormalPerp.y * -playerInputValue.x);
    //         playerBody.velocity = newVelocity;
    //     }
    //     else if (!isGrounded) //If in air
    //     {
    //         // Retain the current horizontal velocity while in the air
    //         newVelocity.Set(playerBody.velocity.x, playerBody.velocity.y);
    //         playerBody.velocity = newVelocity;
    //     }
    // }

    private void ApplyMovement()
    {
        if (isGrounded && !isJumping) // if grounded and not jumping
        {
            newVelocity.Set(movementSpeed * playerInputValue.x, playerBody.velocity.y);
            playerBody.velocity = newVelocity;
        }
        else if (!isGrounded) //If in air
        {
            // Retain the current horizontal velocity while in the air
            newVelocity.Set(playerBody.velocity.x, playerBody.velocity.y);
            playerBody.velocity = newVelocity;
        }
    }

    private void AdjustOrientationToGravity()
    {
        // float movementAngle = playerInputValue.x * movementSpeed * Time.deltaTime;

        // // Rotate around asteroid
        // transform.RotateAround(asteroidCenter, Vector3.forward, movementAngle);
        // facingDirection = playerInputValue.x > 0 ? 1 : 0;

        // // Update up direction to point away from the asteroid's center
        // Vector2 directionFromAsteroid = (transform.position - new Vector3(asteroidCenter.x, asteroidCenter.y, 0)).normalized;
        // transform.up = directionFromAsteroid;

        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, -gravityPoint.gravityDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
