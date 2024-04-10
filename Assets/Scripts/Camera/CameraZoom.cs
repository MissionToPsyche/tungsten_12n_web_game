using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera satelliteCamera;
    private CinemachineVirtualCamera activeCamera;

    private float minZoom = 10f; // Minimum zoom level
    private float maxZoom = 30f; // Maximum zoom level
    private float zoomSpeed = 2f; // Speed of zoom transition
    private float zoomIncrement = 1f; // Amount to zoom on each scroll
    private float targetZoom; // Target zoom level

    void Start()
    {
        activeCamera = playerCamera;
        SetInitialZoomLevel(10f); // Starting zoom level
        SetInitialZoomLevel(targetZoom);
    }

    // -------------------------------------------------------------------

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // -------------------------------------------------------------------
    // Handle events

    private void OnZoomIn(float value)
    {
        // Debug.Log($"OnZoomIn called with value: {value}");
        // if (value > 0)
        // {
        //     Zoom(-zoomIncrement);
        // }
    }

    private void OnZoomOut(float value)
    {
        // Debug.Log($"OnZoomOut called with value: {value}");
        // if (value < 0)
        // {
        //     Zoom(zoomIncrement);
        // }
    }

    // -------------------------------------------------------------------

    public void UpdateActiveCamera(CinemachineVirtualCamera camera)
    {
        activeCamera = camera;
        SetInitialZoomLevel(targetZoom);
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            Zoom(-zoomIncrement); // Zoom in
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            Zoom(zoomIncrement); // Zoom out
        }

        if (activeCamera != null)
        {
            // Smooth zoom transition
            activeCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(activeCamera.m_Lens.OrthographicSize, targetZoom, ref zoomSpeed, 0.1f);
        }
    }

    void Zoom(float increment)
    {
        if (activeCamera != null)
        {
            activeCamera.m_Lens.OrthographicSize = Mathf.Clamp(activeCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
            targetZoom = Mathf.Clamp(activeCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
        }
    }

    public void SetInitialZoomLevel(float initialZoom)
    {
        if (initialZoom >= minZoom && initialZoom <= maxZoom)
        {
            if (activeCamera != null)
            {
                activeCamera.m_Lens.OrthographicSize = initialZoom;
                targetZoom = initialZoom;
            }
        }
    }
}
