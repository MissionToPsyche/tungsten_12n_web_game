using System.Collections;
using UnityEngine;
using Cinemachine;

public class ContextEngine : MonoBehaviour
{
    // Singleton instance for easy access throughout the project.
    public static ContextEngine Instance { get; private set; }

    // Enumeration to determine the current control context - Player or Spaceship.
    public enum ControlState
    {
        Player,
        Spaceship
    }

    // References to other controllers and objects for context switching.
    public PlayerController playerController;
    public SpaceshipController spaceshipController;
    public Transform asteroid; // The current asteroid the player/spaceship interacts with.
    public ControlState currentControlState = ControlState.Spaceship;

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
    }

    private void Start()
    {
        // Initialize the game with the Player as the default control context.
        SetControlState(ControlState.Player);
    }

    private void Update()
    {
        // Toggle between Player and Spaceship control context when the Tab key is pressed.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ControlState desiredState = (currentControlState == ControlState.Player) ? ControlState.Spaceship : ControlState.Player;
            SetControlState(desiredState);
        }
    }

    /// <summary>
    /// Set the current control context to either Player or Spaceship.
    /// </summary>
    public void SetControlState(ControlState state)
    {
        if (state == ControlState.Player)
        {
            currentControlState = ControlState.Player;
            playerController.EnableController();
            spaceshipController.DisableController();
        }
        else if (state == ControlState.Spaceship)
        {
            currentControlState = ControlState.Spaceship;
            spaceshipController.EnableController();
            playerController.DisableController();
        }
    }
}
