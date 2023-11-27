using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody2D : MonoBehaviour
{
    [SerializeField, ReadOnly] private const float GRAVITY_FORCE = 500;
    [SerializeField] private bool gravityApplied;

    [SerializeField] private LayerMask groundLayer;
    private float maxGravityDistance = 10f;
    private float minRotationSpeed = 1f;
    private float maxRotationSpeed = 5f;
    [SerializeField, ReadOnly] private float currentRotationSpeed = 0f;

    public Vector2 GravityDirection
    {
        get
        {
            if (gravityAreas.Count == 0) return Vector2.zero;
            gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody2D rigidBody2D;
    private List<GravityArea2D> gravityAreas;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        gravityAreas = new List<GravityArea2D>();
    }

    private float GetDistanceToGround()
    {
        Vector2 rayDirection = GravityDirection;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GravityDirection, maxGravityDistance, groundLayer);

        // Draw Raycast for debugging
        Debug.DrawRay(transform.position, rayDirection * maxGravityDistance, Color.red);

        if (hit.collider != null)
        {
            return hit.distance;
        }
        return maxGravityDistance; // Return max distance if no ground is detected
    }

    private void FixedUpdate()
    {
        if (gravityApplied)
            rigidBody2D.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode2D.Force);

        float distanceToGround = GetDistanceToGround();
        currentRotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, distanceToGround / maxGravityDistance);

        float targetAngle = Mathf.Atan2(GravityDirection.y, GravityDirection.x) * Mathf.Rad2Deg + 90;
        float smoothedAngle = Mathf.LerpAngle(rigidBody2D.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);
        rigidBody2D.rotation = smoothedAngle;
    }

    public delegate void GravityAreaChangeHandler(GravityArea2D gravityArea);
    public event GravityAreaChangeHandler OnEnterGravityArea;
    public event GravityAreaChangeHandler OnExitGravityArea;

    public void AddGravityArea(GravityArea2D gravityArea)
    {
        gravityAreas.Add(gravityArea);
        OnEnterGravityArea?.Invoke(gravityArea);
        Debug.Log("Entered a new gravity area");
    }

    public void RemoveGravityArea(GravityArea2D gravityArea)
    {
        // gravityAreas.Remove(gravityArea); // disable to prevent player from free-floating in space

        if (gravityAreas.Count == 0)
        {
            OnExitGravityArea?.Invoke(gravityArea);
        }
    }
}
