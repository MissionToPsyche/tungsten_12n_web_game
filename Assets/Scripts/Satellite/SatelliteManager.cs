using UnityEngine;

public class SatelliteManager : MonoBehaviour
{
    public static SatelliteManager instance { get; private set; }

    [Header("Events")]


    [Header("Mutable")]
    public GameObject satellitePrefab;

    [Header("ReadOnly")]
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
            Debug.Log("[GameManager]: Satellite named '" + currentSatelliteObject + "' not found.");
        }
    }

    public void OnSatelliteSpawnTriggered()
    {
        GameObject currentAsteroid = AsteroidManager.instance.GetCurrentAsteroid();
        if (currentAsteroid == null)
        {
            Debug.LogError("[SatelliteManager]: Current asteroid is not set.");
            return;
        }

        GravityFieldEdgePoints gravityFieldEdgePoints = currentAsteroid.GetComponent<GravityFieldEdgePoints>();
        if (gravityFieldEdgePoints == null || gravityFieldEdgePoints.edgePoints == null)
        {
            Debug.LogError("[SatelliteManager]: No GravityFieldEdge component found or edgePoints are not initialized.");
            return;
        }

        Vector2 closestEdgePoint = FindClosestEdgePoint(gravityFieldEdgePoints.edgePoints, PlayerManager.instance.GetPlayerPosition());
        Vector3 spawnPosition = new Vector3(closestEdgePoint.x, closestEdgePoint.y, 0);  // Assuming satellites are on the xy-plane
        GameObject spawnedSatellite = Instantiate(satellitePrefab, spawnPosition, Quaternion.identity, currentAsteroid.transform);

        numberOfSatellites += 1;

        // Setting a custom name for the satellite
        spawnedSatellite.name = "Satellite" + currentAsteroid.GetComponent<Asteroid>().positionTag;

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

        Debug.Log("[SatelliteManager]: Spawned satellite at " + spawnPosition);
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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
