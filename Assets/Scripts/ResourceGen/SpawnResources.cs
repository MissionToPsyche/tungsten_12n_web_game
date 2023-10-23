using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnResources : MonoBehaviour
{
    public static int numberOfObjectsToSpawn = 20;
    public float spawnRadius = 17.5f;
    public Resource[] resourceArray = new Resource[numberOfObjectsToSpawn];
    public GameObject prefab;

    private void Start()
    {
        SpawnObjects();
    }


    private void SpawnObjects()
    {
        for (int iter = 0; iter < numberOfObjectsToSpawn; iter++)
        {
            int infLoopProtector = 0;

            // Calculate a valid random position relative to the SimpleGen's GameObject position
            Vector2 randomPosition = getRandomPosition();
            while (IsValidPosition(randomPosition, iter) == false && infLoopProtector < 100 && iter != 0)
            {
                randomPosition = getRandomPosition();
                infLoopProtector += 1;
                if (infLoopProtector == 100)
                    Debug.Log("spawnResources.cs --: SpawnObjects() :-- While Loop Failed to find a Unique Position to spawn a resource!");
            }

            Resource addedResource = getRandomResource(randomPosition);

            GameObject newResourceObject = Instantiate(prefab, addedResource.Position + (Vector2)transform.position, Quaternion.identity);
            setNewResourceObject(newResourceObject, addedResource);

            newResourceObject.transform.parent = transform.parent;
            newResourceObject.name = $"{addedResource.Name}_" + iter;
            resourceArray[iter] = addedResource;
        }
    }

    //Makes the resource the desired color
    public void setNewResourceObject(GameObject newResourceObject, Resource addedResource)
    {
        SpriteRenderer spriteRenderer = newResourceObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = addedResource.Color;
        spriteRenderer.sortingLayerName = "Foreground";
    }

    public Resource getRandomResource(Vector2 Pos)
    {
        int seed = Random.Range(0, 2);
        if (seed == 1)
        {
            return new Iron(Pos);
        }
        else
        {
            return new Nickel(Pos);
        }
    }

    //Checks to make sure resources arent on top of eachother, i is for bug fixing
    private bool IsValidPosition(Vector3 position, int i)
    {
        float minDistanceBetweenObjects = 4f;
        foreach (Resource resource in resourceArray)
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

    private Vector2 getRandomPosition()
    {
        float r = spawnRadius * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
        float theta = Random.Range(0.0f, 1.0f) * 2 * Mathf.PI;

        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        Vector2 randPosition = new Vector2(x, y);
        return randPosition;
    }
}
