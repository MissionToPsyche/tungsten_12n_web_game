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
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 1, 1, 1); // Fully opaque white
        lineRenderer.endColor = new Color(1, 1, 1, 1); // Fully opaque white
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
            RaycastHit2D hitResource = PerformResourceScan(rayDirection, rayLength);
            UpdateLineRenderer(hitResource, rayDirection, rayLength);
        }
        else
        {
            DisableLineRenderer();
        }
    }

    private RaycastHit2D PerformResourceScan(Vector2 direction, float length)
    {
        RaycastHit2D hitUndiscovered = Physics2D.Raycast(transform.position, direction, length, LayerMask.GetMask("UndiscoveredResource"));
        if (hitUndiscovered.collider != null)
        {
            DiscoverResource(hitUndiscovered);
        }

        RaycastHit2D hitDiscovered = Physics2D.Raycast(transform.position, direction, length, LayerMask.GetMask("DiscoveredResource"));
        return hitUndiscovered.collider ? hitUndiscovered : hitDiscovered;
    }

    private void DiscoverResource(RaycastHit2D hit)
    {
        Debug.Log("Undiscovered resource detected and converted to Discovered Resource.");
        hit.collider.gameObject.layer = LayerMask.NameToLayer("DiscoveredResource");
        hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Resource";
        SoundFXManager.Instance.PlaySound(SFX.Satellite.Scan, transform, 1f);
    }

    private void UpdateLineRenderer(RaycastHit2D hit, Vector2 direction, float length)
    {
        float closestHitDistance = hit.collider ? hit.distance : length;
        Vector3 endPosition = transform.position + (Vector3)direction * closestHitDistance;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("DiscoveredResource"))
        {
            Color detectedColor = hit.collider.gameObject.GetComponent<SpriteRenderer>().color;
            Debug.Log($"Setting Line Renderer color to: {detectedColor}");
            lineRenderer.startColor = lineRenderer.endColor = detectedColor;
        }
        else
        {
            lineRenderer.startColor = lineRenderer.endColor = Color.white;
        }
    }

    private void DisableLineRenderer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.startColor = lineRenderer.endColor = Color.white;
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
