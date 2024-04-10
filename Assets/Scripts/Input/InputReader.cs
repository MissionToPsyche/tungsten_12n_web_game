using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]

public class InputReader :
    ScriptableObject,
    InputSystem.IGameplayActions,
    InputSystem.IPlayerActions,
    InputSystem.ISatelliteActions,
    InputSystem.IUIActions,
    InputSystem.IRobotBuddyActions
{
    private InputSystem inputSystem;

    private Control.State lastControlState;
    private Control.State currentControlState;

    private Dictionary<Control.State, InputActionMap> stateToActionMap;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Gameplay.SetCallbacks(this);
            inputSystem.Player.SetCallbacks(this);
            inputSystem.Satellite.SetCallbacks(this);
            inputSystem.UI.SetCallbacks(this);
            inputSystem.RobotBuddy.SetCallbacks(this);
        }

        stateToActionMap = new Dictionary<Control.State, InputActionMap>
        {
            { Control.State.Player, inputSystem.Player },
            { Control.State.Satellite, inputSystem.Satellite },
            { Control.State.UI, inputSystem.UI },
            { Control.State.RobotBuddyAlpha, inputSystem.RobotBuddy},
            { Control.State.RobotBuddyBeta, inputSystem.RobotBuddy}
        };

        SetControlState(Control.State.Player);
    }

    public void SetControlState(Control.State targetState)
    {
        foreach (var state in stateToActionMap)
        {
            if (state.Key == targetState)
            {
                state.Value.Enable();
            }
            else
            {
                state.Value.Disable();
            }
        }

        // Directly control the state of the Gameplay action map depending on the UI context
        if (targetState != Control.State.UI)
        {
            inputSystem.Gameplay.Enable();
            // Debug.Log("Enabled action map: Gameplay");
        }
        else
        {
            inputSystem.Gameplay.Disable();
            // Debug.Log("Disabled action map: Gameplay");
        }

        if (targetState == Control.State.RobotBuddyAlpha)
        {
            inputSystem.RobotBuddy.Enable();
        }

        lastControlState = currentControlState;
        currentControlState = targetState;
        //Debug.Log($"Current control state set to: {currentControlState}");
    }

    // -------------------------------------------------------------------
    // Define events

    // [Header("Gameplay Events")]
    [SerializeField] private ControlStateEvent ControlStateUpdated;
    [SerializeField] private VoidEvent ZoomIn;
    [SerializeField] private VoidEvent ZoomOut;

    [Header("Movement Events")]
    [SerializeField] private Vector2Event PlayerMove;
    [SerializeField] private BoolEvent PlayerJump;
    [SerializeField] private BoolEvent PlayerSprint;
    [SerializeField] private BoolEvent PlayerCrouch;
    [SerializeField] private BoolEvent PlayerInteract;
    [Header("Player UI Events")]
    [SerializeField] private VoidEvent PlayerBuildOverlay;
    [SerializeField] private VoidEvent PlayerBuildOverlayCycleLeft;
    [SerializeField] private VoidEvent PlayerBuildOverlayCycleRight;
    [SerializeField] private VoidEvent PlayerInventoryOverlay;
    [SerializeField] private VoidEvent PlayerObjectiveOverlay;
    [Header("Satellite Events")]
    [SerializeField] private Vector2Event SatelliteMove;
    [SerializeField] private BoolEvent SatelliteScan;

    [Header("UI Events")]
    [SerializeField] private VoidEvent GamePause;
    [SerializeField] private VoidEvent GameResume;
    [Header("Robot Buddy")]
    [SerializeField] private Vector2Event RobotBuddyMove;
    [SerializeField] private BoolEvent RobotBuddyInteract;

    // -------------------------------------------------------------------
    // Gameplay action map

    public void OnSwitchControlState(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentControlState == Control.State.Player)
            {
                SetControlState(Control.State.Satellite);
            }
            else if (currentControlState == Control.State.Satellite)
            {
                if (RobotManager.instance.GetRobotAlphaBuilt())
                {
                    SetControlState(Control.State.RobotBuddyAlpha);
                }
                else
                {
                    SetControlState(Control.State.Player);
                }
            }
            else if (currentControlState == Control.State.RobotBuddyAlpha)
            {
                if (RobotManager.instance.GetRobotBetaBuilt())
                {
                    SetControlState(Control.State.RobotBuddyBeta);
                }
                else
                {
                    SetControlState(Control.State.Player);
                }
            }
            else if (currentControlState == Control.State.RobotBuddyBeta)
            {
                SetControlState(Control.State.Player);
            }
            ControlStateUpdated.Raise(currentControlState);
        }
    }

    public void OnGamePause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(Control.State.UI);
            GamePause.Raise();
        }
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // ZoomIn.Raise();
        }
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // ZoomOut.Raise();
        }
    }

    // -------------------------------------------------------------------
    // Player action map

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        PlayerMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnPlayerSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerSprint.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerSprint.Raise(false);
        }
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerJump.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerJump.Raise(false);
        }
    }

    public void OnPlayerCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerCrouch.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerCrouch.Raise(false);
        }
    }

    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerInteract.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerInteract.Raise(false);
        }
    }

    public void OnPlayerBuildOverlay(InputAction.CallbackContext context)
    {
        PlayerBuildOverlay.Raise();
    }
    public void OnPlayerBuildOverlayCycleLeft(InputAction.CallbackContext context)
    {
        PlayerBuildOverlayCycleLeft.Raise();
    }
    public void OnPlayerBuildOverlayCycleRight(InputAction.CallbackContext context)
    {
        PlayerBuildOverlayCycleRight.Raise();
    }

    public void OnPlayerInventoryOverlay(InputAction.CallbackContext context)
    {
        PlayerInventoryOverlay.Raise();
    }

    public void OnPlayerObjectiveOverlay(InputAction.CallbackContext context)
    {
        PlayerObjectiveOverlay.Raise();
    }

    // -------------------------------------------------------------------
    // Satellite action map

    public void OnSatelliteMove(InputAction.CallbackContext context)
    {
        SatelliteMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnSatelliteScan(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SatelliteScan.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            SatelliteScan.Raise(false);
        }
    }

    // -------------------------------------------------------------------
    // UI action map

    public void OnGameResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(lastControlState);
            GameResume.Raise();
        }
    }

    // -------------------------------------------------------------------
    // RobotBuddy action map

    public void OnRobotBuddyMove(InputAction.CallbackContext context)
    {
        RobotBuddyMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnRobotBuddyInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RobotBuddyInteract.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            RobotBuddyInteract.Raise(false);
        }
    }
}
