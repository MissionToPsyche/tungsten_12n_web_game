using UnityEngine;

public class Character : MonoBehaviour
{
    //initializing and declaring necessary variables
    public float jumpForce = 7.0f;
    [SerializeField] private Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    //initialize animation and rigid body on start
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
            animator.SetTrigger("Jump-Press");
        }
        //update the animation when the interaction key, 'e' is pressed
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("The e key has been pressed.");
            animator.SetTrigger("Interaction-Press");
        }
        //update the animation when the crouch key, 'LeftControl' is held 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("The left control key has been pressed.");
            animator.SetBool("Crouch-Hold", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch-Hold", false);
        }
        //make sure the character is facing the correct way after walking
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
