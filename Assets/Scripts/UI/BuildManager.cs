using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] GameObject buildOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive;

    public GameObject prefab;

    private void Start()
    {
        buildOverlay.SetActive(false);
    }

    // -------------------------------------------------------------------

    void OnEnable()
    {
        // Subscribe to events
        inputReader.PlayerBuildOverlay += OnPlayerBuildOverlay;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        inputReader.PlayerBuildOverlay -= OnPlayerBuildOverlay;
    }

    // -------------------------------------------------------------------
    // Handle events

    private void OnPlayerBuildOverlay()
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
