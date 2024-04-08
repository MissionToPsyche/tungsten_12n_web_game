using System.Collections;
using UnityEngine;
using Cinemachine;

public class ContextEngine : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject satelliteObject;
    [SerializeField] private GameObject robotBuddyAlphaObject;
    [SerializeField] private GameObject robotBuddyBetaObject;
    [SerializeField, ReadOnly] private GameObject currentObject;

    // Camera
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyAlphaCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyBetaCamera;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera currentCamera;
    [SerializeField, ReadOnly] private float cameraRotationSpeed = 2.5f;


    private void Start()
    {
        currentCamera = playerCamera;
        currentObject = playerObject;
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

    public void OnControlStateUpdated(Control.State currentControlState)
    {
        if (currentControlState == Control.State.Player)
        {
            SetCamerasLowPrio(satelliteCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);
            playerCamera.Priority = 100;
            currentCamera = playerCamera;
            currentObject = playerObject;
        }
        else if (currentControlState == Control.State.Satellite)
        {
            SetCamerasLowPrio(playerCamera, robotBuddyAlphaCamera, robotBuddyBetaCamera);

            satelliteCamera.Priority = 100;
            Transform satelliteTransform = GameManager.instance.GetCurrentSatellite().transform;
            satelliteCamera.Follow = satelliteTransform;
            satelliteCamera.LookAt = satelliteTransform;
            currentCamera = satelliteCamera;
            currentObject = satelliteObject;
        }
        else if(currentControlState == Control.State.RobotBuddyAlpha){
            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyBetaCamera);

            robotBuddyAlphaCamera.Priority = 100;
            currentCamera = robotBuddyAlphaCamera;
            currentObject = robotBuddyAlphaObject;
        }else if(currentControlState == Control.State.RobotBuddyBeta){
            SetCamerasLowPrio(playerCamera, satelliteCamera, robotBuddyAlphaCamera);

            robotBuddyBetaCamera.Priority = 100;
            currentCamera = robotBuddyBetaCamera;
            currentObject = robotBuddyBetaObject;
        }
    }

    // -------------------------------------------------------------------

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
