using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public List<Asteroid> asteroidsList = new List<Asteroid>();
    public GameObject prefab;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Asteroid asteroid = child.GetComponent<Asteroid>();
            if (asteroid != null && child != null)
            {
                //use InstatiateAsteroid to
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
