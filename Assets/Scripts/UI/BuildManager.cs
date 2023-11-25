using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader input;

    [SerializeField] GameObject buildOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive;

    public GameObject prefab;

    void Start()
    {
        // Set up event handlers
        input.BuildOverlayEvent += HandleBuildOverlay;

        buildOverlay.SetActive(false);
    }

    // -------------------------------------------------------------------
    // Event handlers

    private void HandleBuildOverlay()
    {
        isOverlayActive = !isOverlayActive;
        buildOverlay.SetActive(isOverlayActive);
    }

    // -------------------------------------------------------------------

    public void SpawnNewEntity()
    {
        Vector3 screenPos = new Vector3(375, 285, 10f); 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
        buildOverlay.SetActive(false);
    }
}
