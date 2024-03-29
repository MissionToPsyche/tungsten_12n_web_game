using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour
{
    public List<GameObject> prefabsToChooseFrom;
    public GameObject parentObject; // Parent GameObject to contain the instantiated prefab

    public Vector3 desiredScale = new Vector3(2f, 2f, 2f); 

    private GameObject instantiatedPrefab; // Hold reference to the instantiated prefab
    public MiniGameCameraSwitcher cameraSwitcher;

    public VoidEvent isFixedEvent;

    // Method to generate and place a prefab inside another object
    public void GenerateAndPlacePrefab()
    {
        if (prefabsToChooseFrom.Count == 0)
        {
            Debug.LogError("No prefabs assigned to the list.");
            return;
        }

        // Check if there's already an instantiated prefab
        if (instantiatedPrefab != null)
        {
            Debug.LogWarning("Prefab already instantiated. Cannot generate another prefab until the current one is destroyed.");
            return;
        }

        // Randomly choose a prefab from the list
        GameObject prefabToInstantiate = prefabsToChooseFrom[Random.Range(0, prefabsToChooseFrom.Count)];

        // Instantiate the chosen prefab inside the parent GameObject
        instantiatedPrefab = Instantiate(prefabToInstantiate, parentObject.transform);

        // Set the scale of the instantiated prefab
        instantiatedPrefab.transform.localScale = desiredScale;

        // Switch camera to mini game camera
        cameraSwitcher.SwitchToMiniGameCamera();
    }

    // Method to destroy the instantiated prefab
    public void DestroyInstantiatedPrefab()
    {
        if (instantiatedPrefab != null)
        {
            // Switch back to the player camera
            isFixedEvent.Raise();
            
            cameraSwitcher.SwitchBackFromMiniGameCamera();

            // Destroy the instantiated prefab
            Destroy(instantiatedPrefab);

            // Set the reference to null since the prefab is destroyed
            instantiatedPrefab = null;
        }
        else
        {
            Debug.LogWarning("No instantiated prefab to destroy.");
        }
    }
}
