using System.Collections;
using UnityEngine;
using Cinemachine;

public class ContextEngine : MonoBehaviour
{
    // Objects
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject satelliteObject;
    [SerializeField, ReadOnly] private GameObject currentObject;

    // Camera
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
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
            playerCamera.Priority = 100;
            satelliteCamera.Priority = 0;
            currentCamera = playerCamera;
            currentObject = playerObject;
        }
        else if (currentControlState == Control.State.Satellite)
        {
            playerCamera.Priority = 0;
            satelliteCamera.Priority = 100;
            currentCamera = satelliteCamera;
            currentObject = satelliteObject;
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
}
