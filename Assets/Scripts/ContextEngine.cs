using System.Collections;
using UnityEngine;
using Cinemachine;

public class ContextEngine : MonoBehaviour
{
    public static ContextEngine Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        // If the instance already exists and it's not this, destroy this and return
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Else, this instance becomes the singleton instance
        Instance = this;
        DontDestroyOnLoad(gameObject); // This will make the instance persists across scenes
    }

    // Enum to track control state
    public enum ControlState
    {
        Player,
        Spaceship
    }

    // Public variables set through Unity Editor
    public Transform asteroid;
    public Transform player;
    public Transform spaceship;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera spaceshipCamera;
    public ControlState currentControlState = ControlState.Spaceship;

    private float cameraReorientationSpeed = 5f;

    private void Start()
    {
        // SetControlState(ControlState.Player);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Tab))
        // {
        //     // If the current state is Player, switch to Spaceship and vice versa.
        //     ControlState desiredState = (currentControlState == ControlState.Player) 
        //                                 ? ControlState.Spaceship 
        //                                 : ControlState.Player;
        //     SetControlState(desiredState);
        // }

        // Reorient the active camera based on context
        ReorientCameraBasedOnContext();
    }

    public void SetControlState(ControlState state)
    {
        // Based on the desired state, set the appropriate parameters
        if (state == ControlState.Player)
        {
            currentControlState = ControlState.Player;

            spaceshipCamera.Priority = 0;
            playerCamera.Priority = 100;

            player.transform.SetParent(null); // Detach player from asteroid
            playerCamera.Follow = player.transform;

            spaceship.transform.SetParent(asteroid); // Making spaceship a child of the asteroid
        }
        else if (state == ControlState.Spaceship)
        {
            currentControlState = ControlState.Spaceship;

            spaceshipCamera.Priority = 100;
            playerCamera.Priority = 0;

            spaceship.transform.SetParent(null); // Detach spaceship from asteroid
            spaceshipCamera.Follow = spaceship.transform;

            player.transform.SetParent(asteroid); // Making player a child of the asteroid
        }

        // AdjustPlayerOrientation();
        ForceReorientCamera(); // Whenever we switch control state, we force a reorientation immediately
    }

    void ReorientCameraBasedOnContext()
    {
        CinemachineVirtualCamera activeCamera = (currentControlState == ControlState.Player) ? playerCamera : spaceshipCamera;
        Transform focus = activeCamera.Follow;
        
        if (focus)
        {
            Vector2 toFocus = focus.position - asteroid.position;
            float targetAngle = Mathf.Atan2(toFocus.y, toFocus.x) * Mathf.Rad2Deg - 90f;  // -90f to make it upright
            float currentAngle = activeCamera.transform.eulerAngles.z;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * cameraReorientationSpeed);

            activeCamera.transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }
    }

    void ForceReorientCamera()
    {
        CinemachineVirtualCamera activeCamera = (currentControlState == ControlState.Player) ? playerCamera : spaceshipCamera;
        Transform focus = activeCamera.Follow;

        if (focus)
        {
            Vector2 toFocus = focus.position - asteroid.position;
            float targetAngle = Mathf.Atan2(toFocus.y, toFocus.x) * Mathf.Rad2Deg - 90f;  // -90f to make it upright
            activeCamera.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
    }

    // void AdjustPlayerOrientation()
    // {
    //     Vector3 directionToCenter = (player.position - asteroid.position).normalized;
    //     player.up = directionToCenter; // This makes the player's 'up' direction point towards the center of the asteroid
    //     spaceship.up = directionToCenter;
    // }

    // public float GetDirectionMultiplier()
    // {
    //     switch (currentControlState)
    //     {
    //         case ControlState.Player:
    //             return Vector2.Dot(player.up, asteroid.up) > 0 ? 1 : -1;

    //         case ControlState.Spaceship:
    //             return Vector2.Dot(spaceship.up, asteroid.up) > 0 ? 1 : -1;

    //         default:
    //             return 1;  // Default multiplier
    //     }
    // }
}
