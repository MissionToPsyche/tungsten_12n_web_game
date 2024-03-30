using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Progress;

public enum AsteroidClass
{
    S_Class, A_Class, B_Class, C_Class, D_Class, F_Class
}

public class Asteroid : MonoBehaviour
{
    public float size { get; set; }
    private GameObject prefab;
    private List<Resource> resourceList = new List<Resource>();
    public SpawnResources sr;
    public AsteroidClass asteroidClass { get; set; }
    [SerializeField] private SpawnResourceEvent spawnResourceEvent;
    public void InstantiateAsteroid(float Size, int numberOfResources, AsteroidClass asteroidClass, GameObject prefab)
    {
        // Set the size based on the average scale of the GameObject
        size = Size;
        this.asteroidClass = asteroidClass;
        this.prefab = prefab;
        if (prefab != null)
        {
            if (asteroidClass != AsteroidClass.F_Class)
            {
                sr = new SpawnResources(Size, numberOfResources, this);
            }
        }
        else
        {
            Debug.LogError("Prefab not assigned to Asteroid. Please assign the prefab to asteroid Manager in the Unity Editor.");
        }
    }

    public void SpawnResource(Resource addedResource, int iter)
    {
        GameObject newResourceObject = Instantiate(prefab, addedResource.Position + (Vector2)transform.position, Quaternion.identity);
        setNewResourceObject(newResourceObject, addedResource, iter);
    }

    //Makes the resource the desired color
    public void setNewResourceObject(GameObject newResourceObject, Resource addedResource, int iter)
    {
        SpriteRenderer spriteRenderer = newResourceObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = addedResource.Color;
        spriteRenderer.sortingLayerName = "Resource";

        //Debug.Log(this.name + " " + transform.parent.name);
        newResourceObject.transform.localScale = addedResource.depositSize;
        newResourceObject.transform.parent = this.transform;
        newResourceObject.name = $"{addedResource.Name}_" + iter;
        newResourceObject.layer = 8; //Resource Layer

        //Send a dictionary entry with [newresourceObject] = addedResource
        //The key is the actual game object and its entry addedResource has all of resources information
        spawnResourceEvent.Raise(new packet.ResourceGameObjectPacket(newResourceObject, addedResource));
        resourceList.Add(addedResource);
    }

    //Funciton purely used by SpawnResources.cs --: spawnObjects() :-- here so we only ever need one resourceList per asteroid
    public bool IsValidPosition(Vector3 position, int i)
    {
        float minDistanceBetweenObjects = 7f;
        foreach (Resource resource in resourceList)
        {
            if (resource != null)
            {
                float distanceToResource = Vector2.Distance(resource.Position, position);
                //keeps resources from being too close to eachother, as well as outside of the asteroid in edge cases
                if (distanceToResource < minDistanceBetweenObjects)
                {
                    return false; // Position is too close to an existing resource
                }
                //Debug.Log($"Resource_{i} distance to {resource.name}: {distanceToResource}");
            }
        }

        return true;
    }
}
