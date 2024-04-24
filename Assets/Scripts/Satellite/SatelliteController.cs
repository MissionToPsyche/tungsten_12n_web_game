using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class SatelliteController : MonoBehaviour
{
    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private Rigidbody2D satelliteBody;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject parentAsteroid;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;

    SatelliteScan satelliteScan;
    private Vector2[] edgePoints;
    private int currentTargetIndex = 0;
    private float movingSpeed = 5f;
    private float progressBetweenPoints = 0f; // Tracks interpolation progress between points

    private enum State { Autopilot, Manual }
    private State currentState;

    private bool isManualControlEnabled = false;
    private bool lastDirectionWasLeft = true;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSatelliteMove(UnityEngine.Vector2 direction)
    {
        horizontalInput = direction.x;

        if(Mathf.Approximately(satelliteBody.velocity.magnitude, 0))
        {
            satelliteScan.SetIsScanningAllowed(true);
        }
        else
        {
            satelliteScan.SetIsScanningAllowed(false);
        }
    }

    // -------------------------------------------------------------------
    // API

    public void SetParentAsteroid(GameObject asteroid)
    {
        parentAsteroid = asteroid;
    }

    // -------------------------------------------------------------------
    // Class

    void Start()
    {
        satelliteScan = gameObject.GetComponent<SatelliteScan>();
        satelliteScan.SetParentAsteroid(parentAsteroid);

        UpdateEdgePoints();
        currentState = State.Autopilot;
    }

    void UpdateEdgePoints()
    {
        GravityFieldEdgePoints fieldPoints = parentAsteroid.GetComponentInChildren<GravityFieldEdgePoints>();
        if (fieldPoints != null)
        {
            edgePoints = fieldPoints.edgePoints;
            currentTargetIndex = FindClosestEdgePointIndex(edgePoints,PlayerManager.Instance.GetPlayerPosition());
        }
    }

    int FindClosestEdgePointIndex(Vector2[] points, Vector2 position)
    {
        int closestIndex = 0;
        float minDistance = float.MaxValue;
        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector2.Distance(points[i], position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleControlMode();
        }

        if (edgePoints != null && edgePoints.Length > 0)
        {
            if (currentState == State.Manual)
            {
                ManualMove(-horizontalInput);
            }
            else
            {
                MoveAlongEdge();
            }
            UpdateRotation();
        }
    }

    private void ToggleControlMode()
    {
        isManualControlEnabled = !isManualControlEnabled;
        currentState = isManualControlEnabled ? State.Manual : State.Autopilot;

        if (isManualControlEnabled)
        {
            // Capture the current exact position for precise manual control
            satelliteBody.velocity = Vector2.zero; // Stop all movement
        }
        else
        {
            // Adjust the index to make sure the next automatic movement is in the last manual direction
            AdjustTargetIndexForDirection();
        }
    }


    private void ManualMove(float input)
    {
        progressBetweenPoints += input * Time.deltaTime * movingSpeed;
        progressBetweenPoints = Mathf.Clamp01(progressBetweenPoints);

        if (input != 0)
        {
            lastDirectionWasLeft = input < 0;
        }

        Vector2 currentPoint = edgePoints[currentTargetIndex];
        Vector2 nextPoint = edgePoints[(currentTargetIndex + 1) % edgePoints.Length];
        satelliteBody.position = Vector2.Lerp(currentPoint, nextPoint, progressBetweenPoints);

        if (progressBetweenPoints >= 1)
        {
            currentTargetIndex = (currentTargetIndex + 1) % edgePoints.Length;
            progressBetweenPoints = 0;
        }
        else if (progressBetweenPoints <= 0)
        {
            currentTargetIndex = (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length;
            progressBetweenPoints = 1;
        }
    }

    void MoveAlongEdge()
    {
        int nextIndex = lastDirectionWasLeft ?
            (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length :
            (currentTargetIndex + 1) % edgePoints.Length;

        Vector2 targetPoint = edgePoints[currentTargetIndex];
        Vector2 nextPoint = edgePoints[nextIndex];
        float step = movingSpeed * Time.deltaTime;

        satelliteBody.position = Vector2.MoveTowards(satelliteBody.position, targetPoint, step);

        if (Vector2.Distance(satelliteBody.position, targetPoint) < 0.01f)
        {
            currentTargetIndex = nextIndex;
            progressBetweenPoints = 0;
        }
    }

    private void UpdateRotation()
    {
        Vector3 directionToCenter = (transform.position - parentAsteroid.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToCenter);
    }

    private void AdjustTargetIndexForDirection()
    {
        // Determine the closest point in the last direction moved
        if (lastDirectionWasLeft)
        {
            currentTargetIndex = (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length;
        }
        else
        {
            currentTargetIndex = (currentTargetIndex + 1) % edgePoints.Length;
        }
    }

    private void UpdateState(State newState)
    {
        // currentState = newState;
        // UpdateAnimatorParameters();
    }
}
