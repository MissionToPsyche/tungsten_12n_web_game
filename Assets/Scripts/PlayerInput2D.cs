using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput2D : MonoBehaviour 
{
    private PlayerPhysics2D physics;
    private Vector2 playerInputValue;

    private void Awake()
    {
        physics = GetComponent<PlayerPhysics2D>();
    }

    public void OnJump()
    {
        // if (context.started) {
            physics.JumpStart();
        // }
        // if (context.canceled) {
        //     physics.JumpEnd();
        // }
    }

    private void OnMove(InputValue value)
    {
        playerInputValue = value.Get<Vector2>();
        physics.SetMovementDirection((int)playerInputValue.x);

    }
}