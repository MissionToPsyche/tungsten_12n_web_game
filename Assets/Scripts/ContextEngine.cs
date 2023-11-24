using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ContextEngine : MonoBehaviour
{
    // Singleton instance for easy access throughout the project.
    public static ContextEngine Instance { get; private set; }

    [SerializeField] private InputReader input;
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputActionMap satelliteActionMap;

    public enum ControlState
    {
        Player,
        Satellite
    }

    public Transform asteroid;

    public PlayerController playerController;
    //public SpaceshipController spaceshipController;
    public ControlState currentControlState = ControlState.Player;

    // Event to notify other scripts about a change in control context.
    public delegate void ContextChangedHandler(ControlState newState);
    public event ContextChangedHandler OnContextChanged;

    private void Awake()
    {
        // Ensure only one instance of this script exists throughout the game.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance and ensure it persists between scenes.
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Action Maps.
        playerActionMap = inputActions.FindActionMap("Player");
        satelliteActionMap = inputActions.FindActionMap("Satellite");
    }

    private void Start()
    {
        input.SwitchContextEvent += HandleSwitchContext;

        SetControlState(ControlState.Player);
    }

    private void HandleSwitchContext()
    {
        ControlState desiredState = (currentControlState == ControlState.Player) ? ControlState.Satellite : ControlState.Player;
        SetControlState(desiredState);
    }

    public void SetControlState(ControlState state)
    {
        if (state == ControlState.Player)
        {
            currentControlState = ControlState.Player;
            playerController.EnableController();
            //spaceshipController.DisableController();

            playerActionMap.Enable();
            satelliteActionMap.Disable();
        }
        else if (state == ControlState.Satellite)
        {
            currentControlState = ControlState.Satellite;
            //spaceshipController.EnableController();
            playerController.DisableController();

            satelliteActionMap.Enable();
            playerActionMap.Disable();
        }

        // Notify listeners about the context change.
        OnContextChanged?.Invoke(currentControlState);
    }
}
