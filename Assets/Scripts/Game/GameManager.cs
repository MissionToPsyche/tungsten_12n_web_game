using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool playerGrounded; // Current ground state of the player
    [SerializeField, ReadOnly] private Vector2 playerPosition; // Current position of the player in world space
    [SerializeField, ReadOnly] private Vector2 gravityFieldEdgePosition; // GravityField edge position relative to the player
    [SerializeField, ReadOnly] private bool hasBuiltRobotBuddyAlpha = false;
    [SerializeField, ReadOnly] private bool hasBuiltRobotBuddyBeta = false;

    // Not for display

    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerGrounded(bool state)
    {
        playerGrounded = state;
        // Debug.Log("[GameManager]: playerGrounded: " + state);
    }

    public void OnPlayerPositionUpdated(Vector2 position)
    {
        playerPosition = position;
        // Debug.Log("[GameManager]: playerPosition: " + position);
    }

    public void OnGravityFieldEdgeUpdated(Vector2 position)
    {
        // Debug.Log("[GameManager]: gravityFieldEdgePosition: " + position);
        gravityFieldEdgePosition = position;
    }

    public void OnBuildRobotBuddyAlpha()
    {
        hasBuiltRobotBuddyAlpha = true;
    }

    public void OnBuildRobotBuddyBeta()
    {
        hasBuiltRobotBuddyBeta = true;
    }

    public void OnGamePause()
    {
        // pauseMenu.SetActive(true);
        // Debug.Log("[GameManager]: Game paused");
    }

    public void OnGameResume()
    {
        // pauseMenu.SetActive(false);
        // Debug.Log("[GameManager]: Game resumed");
    }

    // -------------------------------------------------------------------
    // API

    public bool GetPlayerGrounded()
    {
        return playerGrounded;
    }

    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public Vector2 GetGravityFieldEdgePosition()
    {
        return gravityFieldEdgePosition;
    }

    public bool GetRobotBuddyAlphaBuilt()
    {
        return hasBuiltRobotBuddyAlpha;
    }

    public bool GetRobotBuddyBetaBuilt()
    {
        return hasBuiltRobotBuddyBeta;
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
