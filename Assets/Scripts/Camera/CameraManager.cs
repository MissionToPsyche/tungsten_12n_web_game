using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyAlphaCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyBetaCamera;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject currentObject;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera currentCamera;
    [SerializeField, ReadOnly] private float cameraRotationSpeed = 2.5f;

    // Not for display

    // -------------------------------------------------------------------
    // Handle events

    public void OnControlStateUpdated(Control.State currentControlState)
    {
        if (currentControlState == Control.State.Player)
        {
            SetCamerasLowPrio(satelliteCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);
            playerCamera.Priority = 100;
            currentCamera = playerCamera;
            currentObject = PlayerManager.instance.GetPlayerObject();
        }
        else if (currentControlState == Control.State.Satellite)
        {
            SetCamerasLowPrio(playerCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);

            satelliteCamera.Priority = 100;
            Transform satelliteTransform = SatelliteManager.instance.GetCurrentSatelliteObject().transform;
            satelliteCamera.Follow = satelliteTransform;
            satelliteCamera.LookAt = satelliteTransform;
            currentCamera = satelliteCamera;
            currentObject = SatelliteManager.instance.GetCurrentSatelliteObject();
        }
        else if(currentControlState == Control.State.RobotBuddyAlpha)
        {
            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyBetaCamera);

            robotBuddyAlphaCamera.Priority = 100;
            currentCamera = robotBuddyAlphaCamera;
            currentObject = RobotManager.instance.GetRobotAlphaObject();
        }
        else if(currentControlState == Control.State.RobotBuddyBeta)
        {
            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyAlphaCamera);

            robotBuddyBetaCamera.Priority = 100;
            currentCamera = robotBuddyBetaCamera;
            currentObject = RobotManager.instance.GetRobotBetaObject();
        }
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        currentCamera = playerCamera;
        currentObject = PlayerManager.instance.GetPlayerObject();
    }

    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        if (currentCamera != null && currentObject != null)
        {
            Quaternion currentRotation = currentCamera.transform.rotation;
            Quaternion targetRotation = currentObject.transform.rotation;
            Quaternion smoothRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

            // Apply the smooth rotation to the camera
            currentCamera.transform.rotation = smoothRotation;
        }
    }

    private void SetCamerasLowPrio(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2, CinemachineVirtualCamera cam3){
        cam1.Priority = 0;
        cam2.Priority = 0;
        cam3.Priority = 0;
    }
}
