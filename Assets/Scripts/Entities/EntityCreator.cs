using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCreator : MonoBehaviour
{
    public GameObject entityPrefab; // The prefab of the entity you want to create
    private List<GameObject> createdEntities = new List<GameObject>(); // List to store created entities

    public Transform cameraTransform;
    public Transform characterTransform;

    public Transform asteroid1; // Reference to asteroid1
    public Transform asteroid2; // Reference to asteroid2


    // Reference to the asteroid colliders
    private Collider asteroid1Collider;
    private Collider asteroid2Collider;


    public float rotationAngle = 0.0f; // The initial rotation angle for the created entity
    public float rotateSpeed = 90.0f; // Rotation speed in degrees per second


    void Start()
    {
        // Get the colliders from the asteroid game objects
        asteroid1Collider = asteroid1.GetComponent<Collider>();
        asteroid2Collider = asteroid2.GetComponent<Collider>();
    }

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
        if (cameraTransform != null && characterTransform != null)
        {

            if (createdEntities.Count > 0)
            {
                EntityMover currentEntityMover = createdEntities[createdEntities.Count - 1].GetComponent<EntityMover>();

                if (currentEntityMover != null)
                {
                    currentEntityMover.SetIsMoving(false);
                }
            }

            float characterZRotation = characterTransform.rotation.eulerAngles.z;
            Vector3 cameraPosition = cameraTransform.position;
            Quaternion characterRotation = characterTransform.rotation;
            Vector3 characterPosition = characterTransform.position;

            float randomX, randomY;
            Vector2 rotatedCoordinates;

           
             randomX = Random.Range(-13f, 13f);
             randomY = Random.Range(1f, 6f);
             rotatedCoordinates = RotateCoordinates(new Vector2(randomX, randomY), characterZRotation);

             Vector3 potentialWorldPosition = new Vector3(
                    rotatedCoordinates.x + cameraPosition.x,
                    rotatedCoordinates.y + cameraPosition.y,
                    0f);

               

              GameObject newEntity = Instantiate(entityPrefab);

                        // Set isMoving to true for the new entity
              EntityMover newEntityMover = newEntity.GetComponent<EntityMover>();
              if (newEntityMover != null)
              {
                newEntityMover.SetIsMoving(true);
              }

                        // Rotate the new entity based on the specified rotation angle
              newEntity.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);

                        // Add the new entity to the list of created entities
             createdEntities.Add(newEntity);

             newEntity.transform.position = potentialWorldPosition;
             newEntity.transform.rotation = characterRotation;

            // Increase the rotation angle for the next created entity
             rotationAngle += rotateSpeed * Time.deltaTime;
            
        }
    }

   

    Vector2 RotateCoordinates(Vector2 coordinates, float angleDegrees)
    {
        // Rotate a 2D point (coordinates) by a given angle (in degrees)
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float x = coordinates.x * Mathf.Cos(angleRadians) - coordinates.y * Mathf.Sin(angleRadians);
        float y = coordinates.x * Mathf.Sin(angleRadians) + coordinates.y * Mathf.Cos(angleRadians);
        return new Vector2(x, y);
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

            EntityMover currentEntityMover = createdEntities[createdEntities.Count - 1].GetComponent<EntityMover>();
            currentEntityMover.SetIsMoving(true);
        }
    }

    void DeleteEntity(int index)
    {
        if (index >= 0 && index < createdEntities.Count)
        {
            if (index == createdEntities.Count - 1)
            {
                DeleteLastEntity();
            }

            else
            {
                GameObject entityToDelete = createdEntities[index];
                createdEntities.RemoveAt(index);
                Destroy(entityToDelete);
            }
        }
    }

   bool IsPositionNearAsteroids(Vector3 position)
    {
        // Get the radii of the asteroid colliders
        float asteroid1Radius = asteroid1Collider.bounds.extents.magnitude;
        float asteroid2Radius = asteroid2Collider.bounds.extents.magnitude;

        float distanceToAsteroid1 = Vector3.Distance(position, asteroid1.position);
        float distanceToAsteroid2 = Vector3.Distance(position, asteroid2.position);

        return distanceToAsteroid1 < asteroid1Radius && distanceToAsteroid2 < asteroid2Radius;
    }

}
