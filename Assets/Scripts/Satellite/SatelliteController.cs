using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class SatelliteController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Rigidbody2D satelliteBody;

    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    private float idleSpeed = 0f;
    private float movingSpeed = 7.5f;

    [SerializeField, ReadOnly] private float currentSpeed;
    private bool isIdle = false;
    private bool isMoving = false;
    private bool isFacingRight = false;

    public GameObject asteroid;

    private enum State { Idle, Moving, Scanning }
    [SerializeField, ReadOnly] private State currentState = State.Idle;

    private void Start()
    {

    }

    public void OnSatelliteMove(Vector2 direction)
    {
        horizontalInput = direction.x;
        isIdle = horizontalInput == 0;

        if (isIdle)
        {
            UpdateState(State.Idle);
        }
        else
        {
            UpdateState(State.Moving);
        }
    }

    public void OnSatelliteScan(bool scanning)
    {
        if (scanning)
        {
            if (currentState != State.Idle) return;
            UpdateState(State.Scanning);
        }
        else
        {
            if (currentState == State.Scanning)
            {
                UpdateState(State.Idle);
            }
        }
    }

    private void UpdateState(State newState)
    {
        currentState = newState;
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        // Reset all animator parameters
        // animator.SetBool("isIdle", false);
        // animator.SetBool("isMoving", false);
        // animator.SetBool("isScanning", false);

        // Update based on current state
        // switch (currentState)
        // {
        //     case State.Idle:
        //         animator.SetBool("isIdle", true);
        //         break;
        //     case State.Moving:
        //         animator.SetBool("isMoving", true);
        //         break;
        //     case State.Scanning:
        //         animator.SetBool("isScanning", true);
        //         break;
        // }
    }

    private void Move()
    {
        switch (currentState)
        {
            case State.Idle:
                currentSpeed = idleSpeed;
                break;
            case State.Moving:
                currentSpeed = movingSpeed;
                break;
        }

        // Calculate the movement direction based on the satellite's current orientation and input
        Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);

        // Move the satellite's rigidbody
        satelliteBody.position += movement;

        // Aim the satellite's local 'up' away from the asteroid
        Vector2 directionToCenter = (Vector2)asteroid.transform.position - (Vector2)satelliteBody.transform.position;
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle + 90);
        satelliteBody.transform.rotation = targetRotation;
    }

    private void Scan()
    {
        if (isIdle)
        {
            // do something
        }
    }

    // User input, animations, moving non-physics objects, game logic
    private void Update()
    {
        UpdateAnimations();

        switch (currentState)
        {
            case State.Idle:
                // Debug.Log("Satellite is IDLE");
                break;
            case State.Moving:
                // Debug.Log("Satellite is MOVING");
                break;
            case State.Scanning:
                // Debug.Log("Satellite is SCANNING");
                break;
        }
    }

    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        // Handle possible inputs
        Move();
        Scan();
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

        // animator.SetBool("isFacingRight", isFacingRight);
        // animator.SetFloat("Horizontal", horizontalInput);
    }

    private void Flip()
    {
        // Switch the way the object is facing
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
