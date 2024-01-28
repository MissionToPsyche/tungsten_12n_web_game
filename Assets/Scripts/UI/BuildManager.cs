using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
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

    }

    private void OnDisable()
    {

    }

    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerBuildOverlay()
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
