using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;

    private GameObject currentAsteroid; // Current asteroid game object the player is on
    private bool satelliteBuilt = false;
    private GameObject currentSatellite; // Current asteroid game object the player is on
    private bool playerGrounded; // Current ground state of the player
    private Vector2 playerPosition; // Current position of the player in world space
    private Vector2 gravityFieldEdgePosition; // GravityField edge position relative to the player

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
        // Find the asteroid GameObject by its name
        currentAsteroid = GameObject.Find(asteroidName);
        if (currentAsteroid == null)
        {
            Debug.LogError("[GameManager]: Asteroid named '" + asteroidName + "' not found.");
            return; // Exit if the asteroid is not found
        }

        // Construct the expected satellite name based on the asteroid's position tag
        Asteroid asteroidComponent = currentAsteroid.GetComponent<Asteroid>();
        if (asteroidComponent == null)
        {
            Debug.LogError("[GameManager]: Asteroid component not found on '" + asteroidName + "'.");
            return; // Exit if the asteroid component is not attached
        }

        string childSatelliteName = "Satellite" + asteroidComponent.positionTag;

        // Find the satellite GameObject by its constructed name
        currentSatellite = GameObject.Find(childSatelliteName);
        if (currentSatellite != null)
        {
            // If the satellite exists, log its name
            Debug.Log("[GameManager]: currentSatellite: " + currentSatellite.name);
        }
        else
        {
            // Log a message if the satellite does not exist
            Debug.Log("[GameManager]: Satellite named '" + childSatelliteName + "' not found.");
        }
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
}
