using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]

public class InputReader :
    ScriptableObject,
    InputSystem.IGameplayActions,
    InputSystem.IPlayerActions,
    InputSystem.ISatelliteActions,
    InputSystem.IUIActions
{
    private InputSystem inputSystem;

    public enum ControlState
    {
        Player,
        Satellite,
        UI
    }

    private ControlState lastControlState;

    private ControlState currentControlState;
    private Dictionary<ControlState, InputActionMap> stateToActionMap;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Gameplay.SetCallbacks(this);
            inputSystem.Player.SetCallbacks(this);
            inputSystem.Satellite.SetCallbacks(this);
            inputSystem.UI.SetCallbacks(this);
        }

        stateToActionMap = new Dictionary<ControlState, InputActionMap>
        {
            { ControlState.Player, inputSystem.Player },
            { ControlState.Satellite, inputSystem.Satellite },
            { ControlState.UI, inputSystem.UI }
        };

        SetControlState(ControlState.Player);
    }

    public void SetControlState(ControlState targetState)
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
        if (targetState != ControlState.UI)
        {
            inputSystem.Gameplay.Enable();
            // Debug.Log("Enabled action map: Gameplay");
        }
        else
        {
            inputSystem.Gameplay.Disable();
            // Debug.Log("Disabled action map: Gameplay");
        }

        lastControlState = currentControlState;
        currentControlState = targetState;
        // Debug.Log($"Current control state set to: {currentControlState}");
    }

    // -------------------------------------------------------------------
    // Define events

    // Gameplay
    public event Action<ControlState> SwitchControlState;
    public event Action PauseGame;
    public event Action<float> ZoomIn;
    public event Action<float> ZoomOut;

    // Player
    public event Action<Vector2> PlayerMove;
    public event Action PlayerSprint;
    public event Action PlayerSprintCancelled;
    public event Action PlayerJump;
    public event Action PlayerJumpCancelled;
    public event Action PlayerCrouch;
    public event Action PlayerCrouchCancelled;
    public event Action PlayerInteract;
    public event Action PlayerInteractCancelled;
    public event Action PlayerBuildOverlay;
    public event Action PlayerInventoryOverlay;
    public event Action PlayerObjectiveOverlay;
    public event Action PlayerBuildOverlayRight;
    public event Action PlayerBuildOverlayLeft;
    // Satellite
    public event Action<Vector2> SatelliteMove;
    public event Action SatelliteScan;
    public event Action SatelliteScanCancelled;

    // UI
    public event Action ResumeGame;

    // -------------------------------------------------------------------
    // Gameplay action map

    public void OnSwitchControlState(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentControlState == ControlState.Player)
            {
                SetControlState(ControlState.Satellite);
            }
            else if (currentControlState == ControlState.Satellite)
            {
                SetControlState(ControlState.Player);
            }

            SwitchControlState?.Invoke(currentControlState);
        }
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(ControlState.UI);
            PauseGame?.Invoke();
        }
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // ZoomIn?.Invoke(context.ReadValue<float>());
        }
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // ZoomOut?.Invoke(context.ReadValue<float>());
        }
    }

    // -------------------------------------------------------------------
    // Player action map

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        PlayerMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPlayerSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerSprint?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerSprintCancelled?.Invoke();
        }
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerJump?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerJumpCancelled?.Invoke();
        }
    }

    public void OnPlayerCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerCrouch?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerCrouchCancelled?.Invoke();
        }      
    }

    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerInteract?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerInteractCancelled?.Invoke();
        }
    }

    public void OnPlayerBuildOverlay(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerBuildOverlay?.Invoke();
        }
    }

    public void OnPlayerBuildOverlayRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerBuildOverlayRight?.Invoke();
        }
    }

    public void OnPlayerBuildOverlayLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerBuildOverlayLeft?.Invoke();
        }
    }
    public void OnPlayerInventoryOverlay(InputAction.CallbackContext context)
    {
        PlayerInventoryOverlay?.Invoke();
    }

    public void OnPlayerObjectiveOverlay(InputAction.CallbackContext context)
    {
        PlayerObjectiveOverlay?.Invoke();
    }


    // -------------------------------------------------------------------
    // Satellite action map

    public void OnSatelliteMove(InputAction.CallbackContext context)
    {
        SatelliteMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSatelliteScan(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SatelliteScan?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            SatelliteScanCancelled?.Invoke();
        }
    }

    // -------------------------------------------------------------------
    // UI action map

    public void OnResumeGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(lastControlState);
            ResumeGame?.Invoke();
        }
    }
}