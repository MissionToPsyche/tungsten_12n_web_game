using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SatelliteScan : MonoBehaviour
{
    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private Rigidbody2D satelliteBody;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject parentAsteroid;
    [SerializeField, ReadOnly] private bool isScanningAllowed = false;
    [SerializeField, ReadOnly] private bool isScanning = false;
    [SerializeField, ReadOnly] private bool resourceDetected = false;

    private LineRenderer lineRenderer;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSatelliteScan(bool scanning)
    {
        if (isScanningAllowed)
        {
            if (scanning)
            {
                StartScanning();
            }
            else
            {
                StopScanning();
            }
        }
    }

    // -------------------------------------------------------------------
    // API

    public void SetIsScanningAllowed(bool canScan)
    {
        isScanningAllowed = canScan;
    }

    public bool GetIsScanningAllowed()
    {
        return isScanningAllowed;
    }

    public void SetParentAsteroid(GameObject asteroid)
    {
        parentAsteroid = asteroid;
    }

    // -------------------------------------------------------------------
    // Base

    private void Start()
    {
        // Add a LineRenderer component dynamically and configure it
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = new Color(0, 1, 0, 1); // Fully opaque green
        lineRenderer.endColor = new Color(0, 1, 0, 1); // Fully opaque green
        lineRenderer.sortingLayerName = "Foreground";
        lineRenderer.sortingOrder = 1;
        lineRenderer.startWidth = 0.05f; // Start width of the line
        lineRenderer.endWidth = 0.05f; // End width of the line
        lineRenderer.positionCount = 2; // Since we are drawing lines between two points
    }

    private void FixedUpdate()
    {
        if (isScanning)
        {
            Vector2 rayDirection = (parentAsteroid.transform.position - transform.position).normalized;
            float rayLength = Vector2.Distance(transform.position, parentAsteroid.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, LayerMask.GetMask("UndiscoveredResource"));

            // Set the positions of the line renderer
            lineRenderer.SetPosition(0, transform.position);
            Vector3 endPosition = transform.position + (Vector3)rayDirection * (hit.collider ? hit.distance : rayLength);
            lineRenderer.SetPosition(1, endPosition);

            if (hit.collider != null)
            {
                if (!resourceDetected)
                {
                    Debug.Log("Undiscovered resource detected!");
                    resourceDetected = true;
                    SoundFXManager.Instance.PlaySound(SFX.Satellite.Scan, transform, 1f);

                    // Update the detected resource's layermask
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("DiscoveredResource");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Resource";
                    Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.green);
                }
            }
            else
            {
                if (resourceDetected)
                {
                    resourceDetected = false;
                    SoundFXManager.Instance.StopSound(SFX.Satellite.Scan);
                }

                Debug.DrawRay(transform.position, rayDirection * rayLength, Color.cyan);
            }
        }
        else
        {
            // Ensure line renderer is invisible when not scanning
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    // -------------------------------------------------------------------
    // Functions

    private void StartScanning()
    {
        isScanning = true;
        Debug.Log("Scan started");
    }

    private void StopScanning()
    {
        isScanning = false;
        Debug.Log("Scan stopped");
    }
}
