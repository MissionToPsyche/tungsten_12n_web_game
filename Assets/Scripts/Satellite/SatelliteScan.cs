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

    }

    private void FixedUpdate()
    {
        if (isScanning)
        {
            Vector2 rayDirection = (parentAsteroid.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, Mathf.Infinity, LayerMask.GetMask("UndiscoveredResource"));

            if (hit.collider != null)
            {
                if (!resourceDetected)
                {
                    Debug.Log("Undiscovered resource detected!");
                    resourceDetected = true;
                    SoundFXManager.Instance.PlaySound(SFX.Satellite.Scan, transform, 1f);
                }

                Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.red);
            }
            else
            {
                SoundFXManager.Instance.StopSoundsOfType(typeof(SFX.Satellite));
                Debug.DrawRay(transform.position, rayDirection * 100, Color.blue);
            }
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
