using System.Collections;
using UnityEngine;
using Cinemachine;

public class ContextEngine : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader input;

    // Objects
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject satelliteObject;
    [SerializeField, ReadOnly] private GameObject currentObject;

    // Camera
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera currentCamera;
    [SerializeField, ReadOnly] private float cameraRotationSpeed = 2.5f;

    public enum ControlState
    {
        Player,
        Satellite
    }

    [SerializeField][ReadOnly] private ControlState currentControlState;

    private void Start()
    {
        // Set up event handlers
        input.SwitchContextEvent += HandleSwitchContext;

        SetControlState(ControlState.Player);
    }

    // -------------------------------------------------------------------
    // Event handlers

    private void HandleSwitchContext()
    {
        ControlState desiredState = (currentControlState == ControlState.Player) ? ControlState.Satellite : ControlState.Player;
        SetControlState(desiredState);
    }

    // -------------------------------------------------------------------

    public void SetControlState(ControlState state)
    {
        if (state == ControlState.Player)
        {
            currentControlState = ControlState.Player;
            playerCamera.Priority = 100;
            satelliteCamera.Priority = 0;
            currentCamera = playerCamera;
            currentObject = playerObject;
        }
        else if (state == ControlState.Satellite)
        {
            currentControlState = ControlState.Satellite;
            playerCamera.Priority = 0;
            satelliteCamera.Priority = 100;
            currentCamera = satelliteCamera;
            currentObject = satelliteObject;
        }
    }

    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        Quaternion currentRotation = currentCamera.transform.rotation;
        Quaternion targetRotation = currentObject.transform.rotation;
        Quaternion smoothRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

        // Apply the smooth rotation to the camera
        currentCamera.transform.rotation = smoothRotation;
    }
}
