using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SatelliteManager : MonoBehaviour
{
    public static SatelliteManager Instance { get; private set; }

    [Header("Events")]


    [Header("Mutable")]
    public GameObject satellitePrefab;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private List<string> activeSatellites = new();
    [SerializeField, ReadOnly] private int numberOfSatellites = 0;
    [SerializeField, ReadOnly] private GameObject currentSatelliteObject;

    // Not for display


    // -------------------------------------------------------------------
    // Handle events

    public void OnCurrentSatelliteChanged(string satelliteName)
    {
        currentSatelliteObject = GameObject.Find(satelliteName);
        if (currentSatelliteObject == null)
        {
            Debug.Log("[SatelliteManager]: Satellite named: " + currentSatelliteObject + " not found.");
        }
    }

    public void OnSatelliteSpawnTriggered()
    {
        GameObject currentAsteroid = AsteroidManager.Instance.GetCurrentAsteroid();
        if (currentAsteroid == null)
        {
            Debug.LogError("[SatelliteManager]: Current asteroid is not set.");
            return;
        }

        // Retrieve satellite data to check if it's already been built
        if (AsteroidManager.Instance.satelliteMap.TryGetValue(currentAsteroid.name, out SatelliteData satelliteData))
        {
            if (satelliteData.isBuilt)
            {
                Debug.Log("[SatelliteManager]: Satellite already built for " + currentAsteroid.name);
                return; // Stop the method if the satellite has already been built
            }

            // Attempt to find the GravityField child within the current asteroid
            GravityFieldEdgePoints gravityFieldEdgePoints = currentAsteroid.GetComponentInChildren<GravityFieldEdgePoints>();
            if (gravityFieldEdgePoints == null || gravityFieldEdgePoints.edgePoints == null)
            {
                Debug.LogError("[SatelliteManager]: No GravityFieldEdge component found or edgePoints are not initialized.");
                return;
            }

            Vector2 closestEdgePoint = FindClosestEdgePoint(gravityFieldEdgePoints.edgePoints, PlayerManager.Instance.GetPlayerPosition());
            Vector3 spawnPosition = new Vector3(closestEdgePoint.x, closestEdgePoint.y, 0);

            if (satellitePrefab == null)
            {
                Debug.LogError("[SatelliteManager]: The satellite prefab is not assigned.");
                return;
            }

            GameObject spawnedSatellite = Instantiate(satellitePrefab, spawnPosition, Quaternion.identity, currentAsteroid.transform);
            if (spawnedSatellite == null)
            {
                Debug.LogError("[SatelliteManager]: Failed to instantiate the satellite prefab.");
                return;
            }
            Debug.Log("[SatelliteManager]: Satellite instantiated successfully.");

            // Update the satellite construction status
            satelliteData.isBuilt = true;  // Mark as built

            // Setting a custom name for the satellite
            spawnedSatellite.name = satelliteData.satelliteName;
            activeSatellites.Add(spawnedSatellite.name);


            // Adjust the scale of the satellite to not inherit the asteroid's scale
            if (currentAsteroid.transform.lossyScale != Vector3.one) // Check if the asteroid's scale is not the default scale
            {
                Vector3 worldScale = spawnedSatellite.transform.lossyScale;
                spawnedSatellite.transform.localScale = new Vector3(
                    spawnedSatellite.transform.localScale.x / currentAsteroid.transform.lossyScale.x,
                    spawnedSatellite.transform.localScale.y / currentAsteroid.transform.lossyScale.y,
                    spawnedSatellite.transform.localScale.z / currentAsteroid.transform.lossyScale.z
                );
            }

            numberOfSatellites++;
            currentSatelliteObject = spawnedSatellite;
            Debug.Log("[SatelliteManager]: Spawned satellite at " + spawnPosition);
        }
        else
        {
            Debug.LogError("[SatelliteManager]: No satellite data found for " + currentAsteroid.name);
        }
    }

    // -------------------------------------------------------------------
    // API

    public int GetNumberOfSatellites()
    {
        return numberOfSatellites;
    }

    public GameObject GetCurrentSatelliteObject()
    {
        return currentSatelliteObject;
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private Vector2 FindClosestEdgePoint(Vector2[] edgePoints, Vector2 playerPosition)
    {
        Vector2 closestPoint = Vector2.zero;
        float minDistance = float.MaxValue;

        foreach (Vector2 point in edgePoints)
        {
            float distance = Vector2.Distance(point, playerPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}
