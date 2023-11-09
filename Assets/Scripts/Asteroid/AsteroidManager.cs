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
            case "Asteroid(0,0)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-1,-1)":
                asteroidClass = AsteroidClass.C_Class;
                break;
            case "Asteroid(-1,-2)":
                asteroidClass = AsteroidClass.F_Class;
                break;
            case "Asteroid(-3,-1)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-2,-2)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-2,-1)":
                asteroidClass = AsteroidClass.A_Class;
                break;
            case "Asteroid(-3,0)":
                asteroidClass = AsteroidClass.B_Class;
                break;
            case "Asteroid(-2,1)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-1,3)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-2,4)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-2,6)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-1,5)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(-1,7)":
                asteroidClass = AsteroidClass.B_Class;
                break;
            case "Asteroid(1,1)":
                asteroidClass = AsteroidClass.D_Class;
                break;
            case "Asteroid(2,2)":
                asteroidClass = AsteroidClass.A_Class;
                break;
            case "Asteroid(0,3)":
                asteroidClass = AsteroidClass.F_Class;
                break;
            case "Asteroid(3,5)":
                asteroidClass = AsteroidClass.S_Class;
                break;
            case "Asteroid(-5,-2)":
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
