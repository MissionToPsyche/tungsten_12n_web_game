using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject asteroid;
    public float moveSpeed = 5f; // Speed at which the player moves around the asteroid
    public float jumpForce = 7.0f;
    [SerializeField] private Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    private Vector2 asteroidCenter => asteroid.transform.position;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Calculate movement angle based on horizontal input
        float movementAngle = horizontalInput * moveSpeed * Time.deltaTime;

        if (ContextEngine.Instance.currentControlState != ContextEngine.ControlState.Player)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Magnitude", 0);
            return;
        }

        if (horizontalInput != 0)
        {
            // Rotate around asteroid
            transform.RotateAround(asteroidCenter, Vector3.forward, movementAngle);
            isFacingRight = horizontalInput > 0;
        }

        // Update up direction to point away from the asteroid's center
       Vector2 directionFromAsteroid = (transform.position - new Vector3(asteroidCenter.x, asteroidCenter.y, 0)).normalized;

        transform.up = directionFromAsteroid;

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump action triggered");
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); // We now jump in the direction that's away from the asteroid
            isGrounded = false;
            animator.SetTrigger("Jump-Press");
        }

        // Interaction animations
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("The e key has been pressed.");
            animator.SetTrigger("Interaction-Press");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("The left control key has been pressed.");
            animator.SetBool("Crouch-Hold", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch-Hold", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            isGrounded = true;
            Debug.Log("Player is on the ground.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            isGrounded = false;
            Debug.Log("Player has left the ground.");
        }
    }
}
