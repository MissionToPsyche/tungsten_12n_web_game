using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityAffectedEntity : MonoBehaviour
{
    public Asteroid asteroid;
    public float gravityStrength = 10.0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ensure Unity's gravity doesn't interfere
    }

    private void Start()
    {
        if (asteroid == null)
        {
            Debug.LogError("Asteroid reference not set on " + gameObject.name);
            return;
        }

        asteroid.RegisterEntity(transform); // Register an entity to the asteroid
    }

    private void FixedUpdate()
    {
        if (asteroid)
        {
            Vector2 gravityDirection = (asteroid.transform.position - transform.position).normalized;
            rb.AddForce(gravityDirection * gravityStrength);
        }
    }

    private void OnDestroy()
    {
        if (asteroid)
        {
            asteroid.UnregisterEntity(transform); // Unregister an entity from the asteroid
        }
    }
}