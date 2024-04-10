using System;
using UnityEngine;

public class GravityFieldEdgePoints : MonoBehaviour
{
    private CircleCollider2D gravityFieldCollider;
    public Vector2Event gravityFieldEdgeUpdated;
    public Vector2[] edgePoints;  // Array to store edge points
    private int numberOfEdgePoints = 64;  // Number of points around the edge (adjustable for granularity)
    private bool edgePointsCalculated = false;  // Flag to ensure edge points are calculated only once

    private void Awake()
    {
        if (gravityFieldEdgeUpdated == null)
        {
            gravityFieldEdgeUpdated = ScriptableObject.CreateInstance<Vector2Event>();
        }
    }

    private void Start()
    {
        Transform gravityField = this.transform.Find("GravityField");
        if (gravityField == null)
        {
            Debug.LogError("GravityField object not found as a child of: " + this.gameObject.name);
            return;
        }

        if (!gravityField.TryGetComponent(out gravityFieldCollider))
        {
            Debug.LogError("Gravity Field Collider not found on GravityField object");
            return;
        }

        if (!edgePointsCalculated)
        {
            // CalculateGravityFieldEdge();
            GenerateEdgePoints();
            edgePointsCalculated = true;  // Set the flag so this only happens once
            // Debug.Log("Edge points calculated for the first time.");
        }
    }

    // private void CalculateGravityFieldEdge()
    // {
    //     Debug.Log("CalculateGravityFieldEdge called.");
    //     if (this.gameObject != null && gravityFieldCollider != null)
    //     {
    //         Vector2 fieldCenter = gravityFieldCollider.transform.position;
    //         Vector2 playerPosition = GameManager.instance.GetPlayerPosition();
    //         Vector2 direction = (playerPosition - fieldCenter).normalized;
    //         float fieldRadius = gravityFieldCollider.radius * Mathf.Max(gravityFieldCollider.transform.lossyScale.x, gravityFieldCollider.transform.lossyScale.y);
    //         Vector2 fieldEdgePoint = fieldCenter + direction * fieldRadius;

    //         gravityFieldEdgeUpdated.Raise(fieldEdgePoint);
    //     }
    //     else
    //     {
    //         Debug.LogError("Asteroid or Player not set properly for calculation.");
    //     }
    // }

    private void GenerateEdgePoints()
    {
        // Debug.Log("GenerateEdgePoints called.");
        edgePoints = new Vector2[numberOfEdgePoints];
        float angleStep = 360f / numberOfEdgePoints;
        float angle = 0;  // Start angle
        Vector2 fieldCenter = gravityFieldCollider.transform.position;
        float effectiveRadius = gravityFieldCollider.radius * Mathf.Max(gravityFieldCollider.transform.lossyScale.x, gravityFieldCollider.transform.lossyScale.y); // Effective radius including scale

        for (int i = 0; i < numberOfEdgePoints; i++)
        {
            edgePoints[i] = new Vector2(
                fieldCenter.x + effectiveRadius * Mathf.Cos(angle * Mathf.Deg2Rad),
                fieldCenter.y + effectiveRadius * Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            angle += angleStep;
            // Debug.Log($"Edge Point {i}: {edgePoints[i]}");
        }
    }

    void FixedUpdate()
    {
        if (edgePoints != null && edgePoints.Length > 0 && gravityFieldCollider != null)
        {
            Vector2 fieldCenter = gravityFieldCollider.transform.position; // Center of the gravity field
            foreach (Vector2 point in edgePoints)
            {
                Debug.DrawLine(fieldCenter, point, Color.red); // Draw line from center to each edge point
            }
        }
    }
}
