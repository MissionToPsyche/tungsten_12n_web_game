using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public static AsteroidManager Instance { get; private set; }

    [Header("Events")]
    [SerializeField] public StringEvent currentSatelliteChanged;

    [Header("Mutable")]
    [SerializeField] public GameObject prefab;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject currentAsteroid;
    [SerializeField, ReadOnly] public List<Asteroid> asteroidsList = new List<Asteroid>();

    // Not for display
    // Dictionary to keep track of satellites for each asteroid
    public Dictionary<string, SatelliteData> satelliteMap = new Dictionary<string, SatelliteData>();


    // -------------------------------------------------------------------
    // Handle events

    public void OnAsteroidReached(string asteroidName)
    {
        // Debug.Log("[AsteroidManager]: Asteroid reached: " + asteroidName);

        currentAsteroid = GameObject.Find(asteroidName);
        if (currentAsteroid == null)
        {
            // Debug.LogError("[GameManager]: Asteroid named '" + asteroidName + "' not found.");
            return;
        }

        // Construct the expected satellite name based on the asteroid's position tag
        if (!currentAsteroid.TryGetComponent<Asteroid>(out var asteroidComponent))
        {
            // Debug.LogError("[GameManager]: Asteroid component not found on '" + asteroidName + "'.");
            return;
        }

        if(SatelliteManager.Instance.GetNumberOfSatellites() > 0)
        {
            currentSatelliteChanged.Raise(satelliteMap[asteroidName].satelliteName);
        }
    }

    // -------------------------------------------------------------------
    // API

    public GameObject GetCurrentAsteroid()
    {
        return currentAsteroid;
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

    void Start()
    {
        foreach (Transform child in transform)
        {
            Asteroid asteroid = child.GetComponent<Asteroid>();
            if (asteroid != null && child != null)
            {
                string satelliteTag = "Satellite" + "_" + asteroid.positionTag;
                satelliteMap.Add(child.name, new SatelliteData(satelliteTag, false));
                FillAsteroidList(asteroid, child);
            }
        }
        //DebugAsteroid();
    }

    private void FillAsteroidList(Asteroid asteroid, Transform child)
    {
        //Asteroid sizes are dependant on its transform
        float sizeToSend = child.transform.localScale.x + child.transform.localScale.y + child.transform.localScale.z;
        AsteroidClass asteroidClass;
        //Reserved for Unique settings
        switch (asteroid.name)
        {
            case "Asteroid_0":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_1":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_2":
                asteroidClass = AsteroidClass.A_Class;
                break;
            case "Asteroid_3":
                asteroidClass = AsteroidClass.C_Class;
                break;
            case "Asteroid_4":
                asteroidClass = AsteroidClass.F_Class;
                break;
            case "Asteroid_5":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_6":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_7":
                asteroidClass = AsteroidClass.A_Class;
                break;
            case "Asteroid_8":
                asteroidClass = AsteroidClass.B_Class;
                break;
            case "Asteroid_9":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_10":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_11":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_12":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_13":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid_14":
                asteroidClass = AsteroidClass.B_Class;
                break;
            case "Asteroid_15":
                asteroidClass = AsteroidClass.F_Class;
                break;
            case "Asteroid_16":
                asteroidClass = AsteroidClass.S_Class;
                break;
            case "Asteroid_17":
                asteroidClass = AsteroidClass.S_Class;
                break;

            default:
                Debug.LogError("AsteroidManager.cs --: FillAsteroidList() :-- Failed to recognize " + asteroid.name);
                asteroidClass = AsteroidClass.F_Class;
                break;
        }
        asteroid.InstantiateAsteroid(sizeToSend * 2.5f, (int)sizeToSend, asteroidClass, prefab);
        asteroidsList.Add(asteroid);
    }

    private void DebugAsteroid()
    {
        foreach (Asteroid asteroid in asteroidsList)
        {
            Debug.Log(asteroid.name + " Size: " + asteroid.size);
            // Add more properties to debug as needed
        }
    }

}
