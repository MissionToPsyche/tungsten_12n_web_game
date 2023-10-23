using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public class EntityCreator : MonoBehaviour
{
    public GameObject entityPrefab; // The prefab of the entity you want to create
    private List<GameObject> createdEntities = new List<GameObject>(); // List to store created entities

    void Update()
    {
        // Check for user input (Ctrl + D) to create a new entity
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            CreateEntity();
        }

        // Check for user input (Ctrl + X) to delete the last created entity
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            DeleteLastEntity();
        }

        // Check for user input (Ctrl + 1, Ctrl + 2, Ctrl + 3, etc.) to delete specific entities
        if (Input.GetKey(KeyCode.LeftControl))
        {
            for (int i = 1; i <= createdEntities.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    DeleteEntity(i - 1);
                }
            }
        }
    }

    void CreateEntity()
    {
        // Instantiate a new entity GameObject from the provided prefab
        GameObject newEntity = Instantiate(entityPrefab);

        // Generate random positions for the new entity
        float randomX = Random.Range(-13f, 13f);
        float randomY = Random.Range(1f, 3f);

        // Set the new entity's position based on the random values
        newEntity.transform.position = new Vector3(randomX, randomY, 0);

        // Add the new entity to the list of created entities
        createdEntities.Add(newEntity);
    }

    void DeleteLastEntity()
    {
        if (createdEntities.Count > 0)
        {
            // Get the last created entity
            GameObject lastEntity = createdEntities[createdEntities.Count - 1];

            // Remove it from the list of created entities
            createdEntities.Remove(lastEntity);

            // Destroy the last entity
            Destroy(lastEntity);
        }
    }

    void DeleteEntity(int index)
    {
        if (index >= 0 && index < createdEntities.Count)
        {
            GameObject entityToDelete = createdEntities[index];
            createdEntities.RemoveAt(index);
            Destroy(entityToDelete);
        }
    }
}
