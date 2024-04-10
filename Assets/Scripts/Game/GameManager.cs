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
    [SerializeField, ReadOnly] private Vector2 gravityFieldEdgePosition;

    // Not for display

    // -------------------------------------------------------------------
    // Handle events

    public void OnGravityFieldEdgeUpdated(Vector2 position)
    {
        // Debug.Log("[GameManager]: gravityFieldEdgePosition: " + position);
        gravityFieldEdgePosition = position;
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

    public Vector2 GetGravityFieldEdgePosition()
    {
        return gravityFieldEdgePosition;
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
