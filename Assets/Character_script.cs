using UnityEngine;

public class Character : MonoBehaviour
{
    public float jumpForce = 7.0f;

    private bool isFacingRight = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Flip the character's sprite based on direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump action triggered");
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; 
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
