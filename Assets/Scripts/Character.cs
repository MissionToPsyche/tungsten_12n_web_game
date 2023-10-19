using UnityEngine;

public class Character : MonoBehaviour
{
    public float jumpForce = 7.0f;
    [SerializeField] private Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        float horizontalInput = Input.GetAxis("Horizontal");

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump action triggered");
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; 
        }
        if (movement.x != 0)
        {
            bool flipped = movement.x > 0; 
            isFacingRight = flipped;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
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
