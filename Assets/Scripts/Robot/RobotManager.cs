using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RobotManager : MonoBehaviour
{
    public static RobotManager instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private GameObject robotAlphaInstance;
    [SerializeField] private GameObject robotBetaInstance;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool robotGrounded;
    [SerializeField, ReadOnly] private Vector2 robotPosition;
    [SerializeField, ReadOnly] private bool hasBuiltRobotBuddyAlpha = false;
    [SerializeField, ReadOnly] private bool hasBuiltRobotBuddyBeta = false;

    // Not for display
    private Camera cam;
    private CinemachineVirtualCamera virtualRobotCamera;
    private RobotBuddyController robotController;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero;
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);

    // -------------------------------------------------------------------
    // Handle events

    public void OnRobotGrounded(bool state)
    {
        robotGrounded = state;
        // Debug.Log("[GameManager]: robotGrounded: " + state);
    }

    public void OnRobotPositionUpdated(Vector2 position)
    {
        robotPosition = position;
        // Debug.Log("[GameManager]: robotPosition: " + position);
    }

    public void OnBuildRobotBuddyAlpha()
    {
        hasBuiltRobotBuddyAlpha = true;
    }

    public void OnBuildRobotBuddyBeta()
    {
        hasBuiltRobotBuddyBeta = true;
    }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     SceneChange(scene);
    // }

    // void OnSceneUnloaded(Scene scene)
    // {
    //     SceneChange(SceneManager.GetSceneByName("AsteroidScene"));
    // }

    // -------------------------------------------------------------------
    // API

    public bool GetRobotGrounded()
    {
        return robotGrounded;
    }

    public Vector2 GetRobotPosition()
    {
        return robotPosition;
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

    void OnEnable()
    {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        // SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    void OnDisable()
    {
        // SceneManager.sceneLoaded -= OnSceneLoaded;
        // SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // private void SceneChange(Scene scene)
    // {
    //     SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
    //     // robotInstance = GameObject.FindWithTag("Robot");
    //     robotController = robotInstance.GetComponent<RobotBuddyController>();
    //     SetScenePosition(scene.name);

    //     virtualRobotCamera = GameObject.FindWithTag("VirtualRobotCamera").GetComponent<CinemachineVirtualCamera>();
    //     virtualRobotCamera.Follow = robotInstance.transform;
    //     virtualRobotCamera.LookAt = robotInstance.transform;

    //     cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    //     cam.transform.position = virtualRobotCamera.transform.position;
    //     cam.transform.rotation = virtualRobotCamera.transform.rotation;
    // }

    // // finds the correct spawn for the newly loaded scene
    // private Transform findCorrectSpawn(String sceneName)
    // {
    //     GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

    //     foreach (GameObject spawnPoint in spawnPoints)
    //     {
    //         if (spawnPoint.scene.name == sceneName)
    //         {
    //             return spawnPoint.GetComponent<Transform>();
    //         }
    //     }
    //     return spawnPoints[0].GetComponent<Transform>();
    // }

    // // set the robots initial position and jump force
    // public void SetScenePosition(String sceneName)
    // {
    //     Transform defaultSpawn = findCorrectSpawn(sceneName);

    //     // put the robot but to their original position if they come back to the asteroid scene
    //     switch (sceneName)
    //     {
    //         case "AsteroidScene":
    //             if (this.lastCoordinates != Vector3.zero && this.lastRotation != Quaternion.Euler(0,0,0))
    //             {
    //                 this.robotInstance.transform.position = this.lastCoordinates;
    //                 this.robotInstance.transform.rotation = this.lastRotation;
    //                 this.robotController.UpdateJumpForce(1.5f);
    //             }
    //             else
    //             {
    //                 // put the robot to the default spawn position in the scene
    //                 this.robotInstance.transform.position = defaultSpawn.position;
    //                 this.robotInstance.transform.rotation = Quaternion.Euler(0,0,0);
    //                 this.robotController.UpdateJumpForce(1.5f);
    //             }
    //             break;

    //         default:
    //             // put the robot to the default spawn position in the scene
    //             this.robotInstance.transform.position = defaultSpawn.position;
    //             this.robotInstance.transform.rotation = Quaternion.Euler(0,0,0);
    //             this.robotController.UpdateJumpForce(0.5f);
    //             break;
    //     }
    // }

    // // set the robots last known coordinates
    // public void setLastPosition()
    // {
    //     this.lastCoordinates = robotInstance.transform.position;
    //     this.lastRotation = robotInstance.transform.rotation;
    // }
}
