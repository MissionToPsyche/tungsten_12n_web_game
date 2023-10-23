using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D crouchDisableCollider;				// A collider that will be disabled when crouching

	const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isGrounded;            // Whether or not the player is grounded.
	const float ceilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rigidBody2D;
	private bool isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
    [SerializeField] private GravityPoint gravityPoint;

	private void Awake()
	{
        rigidBody2D = GetComponent<Rigidbody2D>();
        // gravityPoint = gravityPoint;
	}


	private void FixedUpdate()
	{
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				isGrounded = true;
		}
	}

	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
			{
				crouch = true;
			}
		}

                    // Calculate the gravitational direction.
            Vector2 gravityDirection = (transform.position - gravityPoint.transform.position).normalized;


		//only control the player if grounded or airControl is turned on
		if (isGrounded || airControl)
		{

			// If crouching
			if (crouch)
			{
				// Reduce the speed by the crouchSpeed multiplier
				move *= crouchSpeed;

				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;
			}



            // Move the character perpendicular to the gravity direction.
            Vector2 movementDirection = new Vector2(-gravityDirection.y, gravityDirection.x);
            Vector3 targetVelocity = new Vector3(movementDirection.x * move * 10f, movementDirection.y * move * 10f + rigidBody2D.velocity.y, 0);

            // Smoothing the movement
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            // Ensure the character is facing the right direction based on movement.
            if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
            {
                Flip();
            }


			// // Move the character by finding the target velocity
			// Vector3 targetVelocity = new Vector2(move * 10f, rigidBody2D.velocity.y);
			// // And then smoothing it out and applying it to the character
			// rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// // If the input is moving the player right and the player is facing left...
			// if (move > 0 && !isFacingRight)
			// {
			// 	// ... flip the player.
			// 	Flip();
			// }
			// // Otherwise if the input is moving the player left and the player is facing right...
			// else if (move < 0 && isFacingRight)
			// {
			// 	// ... flip the player.
			// 	Flip();
			// }
		}
		// If the player should jump...
        if (isGrounded && jump)
        {
            Vector2 jumpDirection = -gravityDirection;
            rigidBody2D.AddForce(jumpDirection * jumpForce);
            isGrounded = false;
        }
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}