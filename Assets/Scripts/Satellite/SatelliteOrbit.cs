using UnityEngine;

public class SatelliteOrbit : MonoBehaviour
{
    public GameObject satellite;
    public GameObject asteroid;
    public float orbitSpeed = .075f;
    private CircleCollider2D orbitCollider;
    private float orbitAngle;

    void Start()
    {
        orbitCollider = asteroid.transform.Find("GravityField").GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        UpdateOrbitRotationAndRaycast();
    }

    void UpdateOrbitRotationAndRaycast()
    {
        // Orbit Mechanics
        orbitAngle += orbitSpeed * Time.fixedDeltaTime;
        float orbitRadius = orbitCollider.radius * Mathf.Max(asteroid.transform.localScale.x, asteroid.transform.localScale.y);
        float x = Mathf.Cos(orbitAngle) * orbitRadius + asteroid.transform.position.x;
        float y = Mathf.Sin(orbitAngle) * orbitRadius + asteroid.transform.position.y;
        Vector2 newPosition = new Vector2(x, y);
        satellite.transform.position = newPosition;

        // Aim the satellite's local 'up' away from the asteroid
        Vector2 directionToCenter = (Vector2)asteroid.transform.position - (Vector2)satellite.transform.position;
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle + 90);
        satellite.transform.rotation = targetRotation;

        ShootDebugRaycasts();

        // Raycast towards the center of the asteroid
        RaycastHit2D hit = Physics2D.Raycast(satellite.transform.position, -satellite.transform.up, orbitRadius);
        Debug.DrawRay(satellite.transform.position, -satellite.transform.up * orbitRadius, Color.blue); // Draw a blue line towards the center if no hit

        // if (hit.collider != null)
        // {
        //     Debug.DrawLine(satellite.transform.position, hit.point, Color.blue); // Draw a blue line towards the hit point
        //     Debug.Log("Raycast hit: " + hit.collider.name);
        // }
        // else
        // {
        // }
    }

    void ShootDebugRaycasts()
    {
        float rayLength = 2f; // Length of the debug rays
        Debug.DrawRay(satellite.transform.position, satellite.transform.up * rayLength, Color.green); // "Up" direction
        Debug.DrawRay(satellite.transform.position, satellite.transform.right * rayLength, Color.red); // "Right" direction
    }
}
