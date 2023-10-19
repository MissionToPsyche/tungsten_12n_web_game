using UnityEngine;

public class Character : MonoBehaviour
{
    public Asteroid asteroid;
    public float jumpForce = 5.0f;
    [SerializeField] private Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    public bool IsCharacterGrounded()
    {
        return isGrounded;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX; // Lock the X position
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Always update the direction based on input, but do not move in X direction
        if (horizontalInput != 0)
        {
            bool flipped = horizontalInput > 0; 
            isFacingRight = flipped;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
        }

        // Disable character inputs when not in focus
        if (ContextEngine.Instance.currentControlState != ContextEngine.ControlState.Character)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Magnitude", 0);
            return;
        }

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Vector2 jumpDirection = (transform.position - asteroid.transform.position).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; 
        }

        if (movement.x == 0 && movement.magnitude == 0 && isFacingRight) 
        { 
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f)); 
            animator.SetFloat("Direction", 1); }
        else if (movement.x == 0 && movement.magnitude == 0 && !isFacingRight) 
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            animator.SetFloat("Direction", -1); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            isGrounded = true;
            Debug.Log("Character is on the ground.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            isGrounded = false;
            Debug.Log("Character has left the ground.");
        }
    }

}
