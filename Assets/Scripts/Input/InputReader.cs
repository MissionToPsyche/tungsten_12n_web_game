using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, InputSystem.IPlayerActions, InputSystem.ISatelliteActions, InputSystem.IUIActions
{
    private InputSystem inputSystem;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Player.SetCallbacks(this);
            inputSystem.Satellite.SetCallbacks(this);
            inputSystem.UI.SetCallbacks(this);
        }

        SetPlayer();
    }

    public void SetPlayer()
    {
        inputSystem.Player.Enable();
        inputSystem.UI.Disable();
    }

    public void SetUI()
    {
        inputSystem.Player.Disable();
        inputSystem.UI.Enable();
    }

    // Movement
    public event Action<float> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action CrouchEvent;
    public event Action CrouchCancelledEvent;

    // Interactions
    public event Action InteractEvent;
    public event Action InteractCancelledEvent;

    // Overlays
    public event Action InventoryEvent;
    public event Action SwitchContextEvent;
    public event Action MissionOverlayEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CrouchEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            CrouchCancelledEvent?.Invoke();
        }      
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            InteractCancelledEvent?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        InventoryEvent?.Invoke();
    }

    public void OnSwitchContext(InputAction.CallbackContext context)
    {
        SwitchContextEvent?.Invoke();
    }

    public void OnMissionOverlay(InputAction.CallbackContext context)
    {
        MissionOverlayEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseEvent?.Invoke();
        SetUI();
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        ResumeEvent?.Invoke();
        SetPlayer();
    }
}
