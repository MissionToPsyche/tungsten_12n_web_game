using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour 
{
    [Tooltip("This is the root of the player character.")]
    [SerializeField] private Transform characterRoot;
    [Tooltip("This is the pivot of the character model that will be rotated left and right.")]
    [SerializeField] private Transform characterPivot;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float maxTorque = 500;
    [SerializeField] private float torqueAcceleration = 250;
    private float torque = 0;
    private int moveInput = 0;
    private int direction = 1;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer = 0;
    private bool jumpStartTriggered = false;
    private bool jumpEndTriggered = false;
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Vector3 gravitationalDirection = Vector3.down; // default to Unity's down if not affected by any gravity source.

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = characterRoot.GetComponentInChildren<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        Collider2D[] cols = characterRoot.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
        }

        // Assuming the player starts within the asteroid's gravitational field
        GravityPoint gravitySource = FindObjectOfType<GravityPoint>();
        if(gravitySource != null) {
            gravitySource.OnGravityEffect += UpdateGravityDirection;
        }
    }

    private void UpdateGravityDirection(Vector3 gravityDirection, float gravitationalPower)
    {
        this.gravitationalDirection = gravityDirection;
    }

    private void OnDestroy() 
    {
        GravityPoint gravitySource = FindObjectOfType<GravityPoint>();
        if(gravitySource != null) {
            gravitySource.OnGravityEffect -= UpdateGravityDirection;
        }
    }

    private void FixedUpdate()
    {
        characterRoot.position = transform.position;
        // OrientPlayerToGravity();
        RotateFace();
        CheckGroundBelow();
    }

    private void OrientPlayerToGravity() 
    {
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, -gravitationalDirection);
        characterRoot.rotation = Quaternion.Slerp(characterRoot.rotation, targetRotation, 10 * Time.deltaTime);
    }

    private void Update()
    {
        if (isGrounded) {
            GroundInput();

            if (jumpStartTriggered) {
                isJumping = true;
                jumpTimer = 0;
            }
        } else {
            AirInput();
        }

        JumpingBehaviour();

        // anim.SetFloat("speedX", rb.velocity.sqrMagnitude);
        // anim.SetFloat("speedY", rb.velocity.y);
        // anim.SetBool("isGrounded", isGrounded);
    }

    private void RotateFace()
    {
        Vector3 newRotEuler = characterPivot.localRotation.eulerAngles;
        newRotEuler.y = Mathf.Lerp(newRotEuler.y, direction == 1 ? 0 : 180, 8 * Time.deltaTime);
        characterPivot.localRotation = Quaternion.Euler(newRotEuler);
    }

    private void JumpingBehaviour()
    {
        if (isJumping) {
            Vector2 vel = rb.velocity;
            vel.y = 10;

            if (jumpEndTriggered) {
                vel = rb.velocity;
                vel.y /= 2;
                isJumping = false;
                jumpEndTriggered = false;
            }

            jumpTimer += Time.deltaTime;
            rb.velocity = vel;

            if (jumpTimer >= 0.2f) {
                isJumping = false;
            }
        }
        jumpStartTriggered = false;
        jumpEndTriggered = false;
    }

    private void GroundInput()
    {
        if (moveInput == 1) {
            direction = 1;
            if (torque > 0) torque = 0;
            torque -= torqueAcceleration * Time.deltaTime;
            if (torque < -maxTorque) torque = -maxTorque;
            rb.AddTorque(torque, ForceMode2D.Impulse);
        } else if (moveInput == -1) {
            direction = -1;
            if (torque < 0) torque = 0;
            torque += torqueAcceleration * Time.deltaTime;
            if (torque > maxTorque) torque = maxTorque;
            rb.AddTorque(torque, ForceMode2D.Impulse);
        } else {
            torque = 0;
        }
    }

    private void AirInput()
    {
        if (moveInput == 1) {
            if (torque > 0) torque = 0;
            torque -= torqueAcceleration * Time.deltaTime;
            if (torque < -maxTorque) torque = -maxTorque;
            rb.AddTorque(torque, ForceMode2D.Impulse);
        } else if (moveInput == -1) {
            if (torque < 0) torque = 0;
            torque += torqueAcceleration * Time.deltaTime;
            if (torque > maxTorque) torque = maxTorque;
            rb.AddTorque(torque, ForceMode2D.Impulse);
        }
    }

    private void CheckGroundBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, characterRoot.up * -1, circleCollider.radius + 0.01f, groundLayerMask);
        Debug.DrawLine(transform.position, transform.position + characterRoot.up * -1 * (circleCollider.radius + 0.01f), Color.cyan);
        if (hit) {
            SetGrounded(hit.normal);
        } else {
            isGrounded = false;
        }
    }

    private void SetGrounded(Vector2 _normal)
    {
        isGrounded = true;
        isJumping = false;
        Quaternion newRot = Quaternion.FromToRotation(Vector3.up, _normal);
        characterRoot.rotation = Quaternion.Lerp(characterRoot.rotation, newRot, 10 * Time.deltaTime);
    }

    public void SetMovementDirection(int _direction)
    {
        moveInput = _direction;
    }

    public void JumpStart()
    {
        jumpStartTriggered = true;
    }

    public void JumpEnd() 
    {
        jumpEndTriggered = true;
    }
}