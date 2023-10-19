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
        Character,
        Spaceship
    }

    // Public variables set through Unity Editor
    public Transform asteroid;
    public Transform character;
    public Transform spaceship;
    public CinemachineVirtualCamera characterCamera;
    public CinemachineVirtualCamera spaceshipCamera;
    public ControlState currentControlState = ControlState.Character;

    private float cameraReorientationSpeed = 5f;

    private void Start()
    {
        spaceship.transform.SetParent(asteroid); // Making spaceship a child of the asteroid
    }

    private void Update()
    {
        // Switching context
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchControlState();
        }

        // Reorient the active camera based on context
        ReorientCameraBasedOnContext();
    }

    public void SwitchControlState()
    {
        switch (currentControlState)
        {
            case ControlState.Character:
                currentControlState = ControlState.Spaceship;

                spaceshipCamera.Priority = 100;
                characterCamera.Priority = 0;

                spaceship.transform.SetParent(null); // Detach spaceship from asteroid


                spaceshipCamera.Follow = spaceship.transform; // The spaceship's camera should follow the spaceship.


                character.transform.SetParent(asteroid); // Making character a child of the asteroid

                break;

            case ControlState.Spaceship:
                currentControlState = ControlState.Character;

                spaceshipCamera.Priority = 0;
                characterCamera.Priority = 100;
                    
                character.transform.SetParent(null); // Detach character from asteroid

                characterCamera.Follow = character.transform;

                spaceship.transform.SetParent(asteroid); // Making spaceship a child of the asteroid

                break;
        }

        AdjustPlayerOrientation();

        // Whenever we switch control state, we force a reorientation immediately
        ForceReorientCamera();
    }

    void ReorientCameraBasedOnContext()
    {
        CinemachineVirtualCamera activeCamera = (currentControlState == ControlState.Character) ? characterCamera : spaceshipCamera;
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
        CinemachineVirtualCamera activeCamera = (currentControlState == ControlState.Character) ? characterCamera : spaceshipCamera;
        Transform focus = activeCamera.Follow;

        if (focus)
        {
            Vector2 toFocus = focus.position - asteroid.position;
            float targetAngle = Mathf.Atan2(toFocus.y, toFocus.x) * Mathf.Rad2Deg - 90f;  // -90f to make it upright
            activeCamera.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
    }

    void AdjustPlayerOrientation()
    {
        Vector3 directionToCenter = (character.position - asteroid.position).normalized;
        character.up = directionToCenter; // This makes the character's 'up' direction point towards the center of the asteroid
        spaceship.up = directionToCenter;
    }

    public float GetDirectionMultiplier()
    {
        switch (currentControlState)
        {
            case ControlState.Character:
                return Vector2.Dot(character.up, asteroid.up) > 0 ? 1 : -1;

            case ControlState.Spaceship:
                return Vector2.Dot(spaceship.up, asteroid.up) > 0 ? 1 : -1;

            default:
                return 1;  // Default multiplier
        }
    }
}
