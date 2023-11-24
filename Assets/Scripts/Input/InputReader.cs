using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.ISatelliteActions, GameInput.IUIActions
{
    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Player.SetCallbacks(this);
            gameInput.Satellite.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);
        }

        SetPlayer();
    }

    public void SetPlayer()
    {
        gameInput.Player.Enable();
        gameInput.UI.Disable();
    }
    public void SetUI()
    {
        gameInput.Player.Disable();
        gameInput.UI.Enable();
    }

    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action CrouchEvent;
    public event Action CrouchCancelledEvent;
    public event Action InteractEvent;
    public event Action InventoryEvent;
    public event Action SwitchContextEvent;
    public event Action MissionOverlayEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
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
        InteractEvent?.Invoke();
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
