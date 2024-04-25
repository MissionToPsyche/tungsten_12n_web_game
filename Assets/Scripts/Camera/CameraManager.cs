using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyAlphaCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyBetaCamera;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject currentObject;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera currentCamera;
    [SerializeField, ReadOnly] private bool rotateCameraWithPlayer = true;

    // Not for display
    private float cameraRotationSpeed = 2.5f;
    private float minZoom = 10f; // Minimum zoom level
    private float maxZoom = 30f; // Maximum zoom level
    private float zoomSpeed = 2f; // Speed of zoom transition
    private float zoomIncrement = 1f; // Amount to zoom on each scroll
    private float targetZoom; // Target zoom level
    private Quaternion targetRotation;

    // -------------------------------------------------------------------
    // Handle events

    public void OnRotateCameraWithPlayer()
    {
        rotateCameraWithPlayer = !rotateCameraWithPlayer;

        // Set the target rotation based on the new state
        if (rotateCameraWithPlayer)
        {
            // Target the player's rotation
            if (currentObject != null)
            {
                targetRotation = currentObject.transform.rotation;
            }
        }
        else
        {
            // Target the upright rotation (world up)
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnZoomIn()
    {
        Zoom(-zoomIncrement);
    }

    public void OnZoomOut()
    {
        Zoom(+zoomIncrement);
    }

    public void OnControlStateUpdated(Control.State controlState)
    {
        if (controlState == Control.State.Player)
        {
            SetCamerasLowPrio(satelliteCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);
            playerCamera.Priority = 100;
            currentCamera = playerCamera;
            currentObject = PlayerManager.Instance.GetPlayerObject();
        }
        else if (controlState == Control.State.Satellite)
        {
            SetCamerasLowPrio(playerCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);

            satelliteCamera.Priority = 100;
            Transform satelliteTransform = SatelliteManager.Instance.GetCurrentSatelliteObject().transform;
            satelliteCamera.Follow = satelliteTransform;
            satelliteCamera.LookAt = satelliteTransform;
            currentCamera = satelliteCamera;
            currentObject = SatelliteManager.Instance.GetCurrentSatelliteObject();
        }
        else if (controlState == Control.State.RobotBuddyAlpha)
        {

            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyBetaCamera);

            robotBuddyAlphaCamera.Priority = 100;
            currentCamera = robotBuddyAlphaCamera;
            currentObject = RobotManager.Instance.GetRobotAlphaObject();
        }
        else if (controlState == Control.State.RobotBuddyBeta)
        {
            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyAlphaCamera);

            robotBuddyBetaCamera.Priority = 100;
            currentCamera = robotBuddyBetaCamera;
            currentObject = RobotManager.Instance.GetRobotBetaObject();
        }
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        currentCamera = playerCamera;
        currentObject = PlayerManager.Instance.GetPlayerObject();

        SetInitialZoomLevel(10f);
    }

    void Update()
    {
        playerCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(playerCamera.m_Lens.OrthographicSize, targetZoom, ref zoomSpeed, 0.1f);
    }

    void Zoom(float increment)
    {

        playerCamera.m_Lens.OrthographicSize = Mathf.Clamp(playerCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
        targetZoom = Mathf.Clamp(playerCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
    }

    public void SetInitialZoomLevel(float initialZoom)
    {
        if (initialZoom >= minZoom && initialZoom <= maxZoom)
        {
            if (playerCamera != null)
            {
                playerCamera.m_Lens.OrthographicSize = initialZoom;
                targetZoom = initialZoom;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentCamera != null)
        {
            if (rotateCameraWithPlayer && currentObject != null)
            {
                // Slerp to the player's rotation
                targetRotation = currentObject.transform.rotation;
            }

            // Always Slerp towards the target rotation whether it's the player's or upright
            Quaternion currentRotation = currentCamera.transform.rotation;
            Quaternion smoothRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

            currentCamera.transform.rotation = smoothRotation;
        }
    }

    private void SetCamerasLowPrio(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2, CinemachineVirtualCamera cam3)
    {
        cam1.Priority = 0;
        cam2.Priority = 0;
        cam3.Priority = 0;
    }
}
