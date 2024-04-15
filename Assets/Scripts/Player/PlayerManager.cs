using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject CyberneticToolObject;
    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool playerGrounded;
    [SerializeField, ReadOnly] private Vector2 playerPosition;

    // Not for display
    private Camera cam;
    private CinemachineVirtualCamera virtualPlayerCamera;
    private PlayerController playerController;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero;
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);
    [SerializeField] private GameObject playerUIObject;
    //private PlayerUIManager playerUIManager;
    
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneChange(scene);
    }

    void OnSceneUnloaded(Scene scene)
    {
        SceneChange(SceneManager.GetSceneByName("AsteroidScene"));
    }
    // -------------------------------------------------------------------
    // API

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public bool GetPlayerGrounded()
    {
        return playerGrounded;
    }

    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void SceneChange(Scene scene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
        // playerObject = GameObject.FindWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        SetScenePosition(scene.name);

        virtualPlayerCamera = GameObject.FindWithTag("VirtualPlayerCamera").GetComponent<CinemachineVirtualCamera>();
        virtualPlayerCamera.Follow = playerObject.transform;
        virtualPlayerCamera.LookAt = playerObject.transform;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = virtualPlayerCamera.transform.position;
        cam.transform.rotation = virtualPlayerCamera.transform.rotation;
    }

    // finds the correct spawn for the newly loaded scene
    private Transform findCorrectSpawn(String sceneName)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.scene.name == sceneName)
            {
                return spawnPoint.GetComponent<Transform>();
            }
        }
        return spawnPoints[0].GetComponent<Transform>();
    }

    // set the players initial position and jump force
    public void SetScenePosition(String sceneName)
    {
        Transform defaultSpawn = findCorrectSpawn(sceneName);

        // put the player but to their original position if they come back to the asteroid scene
        switch (sceneName)
        {
            case "AsteroidScene":
                if (this.lastCoordinates != Vector3.zero && this.lastRotation != Quaternion.Euler(0,0,0))
                {
                    this.playerObject.transform.position = this.lastCoordinates;
                    this.playerObject.transform.rotation = this.lastRotation;
                    this.playerController.UpdateJumpForce(1.5f);
                }
                else
                {
                    // put the player to the default spawn position in the scene
                    this.playerObject.transform.position = defaultSpawn.position;
                    this.playerObject.transform.rotation = Quaternion.Euler(0,0,0);
                    this.playerController.UpdateJumpForce(1.5f);
                }
                break;

            default:
                // put the player to the default spawn position in the scene
                this.playerObject.transform.position = defaultSpawn.position;
                this.playerObject.transform.rotation = Quaternion.Euler(0,0,0);
                this.playerController.UpdateJumpForce(0.5f);
                break;
        }
    }

    // set the players last known coordinates
    public void setLastPosition()
    {
        this.lastCoordinates = playerObject.transform.position;
        this.lastRotation = playerObject.transform.rotation;
    }
}
