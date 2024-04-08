using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;

    private GameObject currentAsteroid; // Current asteroid game object the player is on
    private GameObject currentSatellite; // Current asteroid game object the player is on
    private bool playerGrounded; // Current ground state of the player
    private Vector2 playerPosition; // Current position of the player in world space
    private Vector2 gravityFieldEdgePosition; // GravityField edge position relative to the player
    private bool hasBuiltRobotBuddyAlpha = false;
    private bool hasBuiltRobotBuddyBeta = false;

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

    // -------------------------------------------------------------------
    // Handle events

    public void OnAsteroidReached(string asteroidName)
    {
        // currentAsteroid = GameObject.Find(asteroidName);
        // string childSatelliteName = "Satellite" + currentAsteroid.transform.GetComponent<Asteroid>().positionTag;
        // currentSatellite = GameObject.Find(childSatelliteName);
        // Debug.Log("[GameManager]: currentSatellite: " + currentSatellite.name);
    }

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
        pauseMenu.SetActive(true);
        // Debug.Log("[GameManager]: Game paused");
    }

    public void OnGameResume()
    {
        pauseMenu.SetActive(false);
        // Debug.Log("[GameManager]: Game resumed");
    }

    // -------------------------------------------------------------------
    // GameManager API

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

    public GameObject GetCurrentAsteroid()
    {
        return currentAsteroid;
    }

    public bool GetRobotBuddyAlphaBuilt()
    {
        return hasBuiltRobotBuddyAlpha;
    }

    public bool GetRobotBuddyBetaBuilt()
    {
        return hasBuiltRobotBuddyBeta;
    }
}