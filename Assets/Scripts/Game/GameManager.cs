using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    // [Header("Events")]

    // [Header("Mutable")]

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private Control.State currentControlState;

    // Not for display

    // -------------------------------------------------------------------
    // Handle events

    public void OnControlStateUpdated(Control.State controlState)
    {
        currentControlState = controlState;
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

    public Control.State GetCurrentControlState()
    {
        return currentControlState;
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
