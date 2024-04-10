using System.Collections;
using UnityEngine;
using Cinemachine;

public class RobotManager : MonoBehaviour
{
    public static RobotManager instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool hasBuiltAlpha = false;
    [SerializeField, ReadOnly] private bool hasBuiltBeta = false;
    // Alpha
    [SerializeField, ReadOnly] private GameObject robotAlphaObject;
    [SerializeField, ReadOnly] private bool robotAlphaGrounded;
    [SerializeField, ReadOnly] private Vector2 robotAlphaPosition;
    // Beta
    [SerializeField, ReadOnly] private GameObject robotBetaObject;
    [SerializeField, ReadOnly] private bool robotBetaGrounded;
    [SerializeField, ReadOnly] private Vector2 robotBetaPosition;

    // Not for display
    private Camera cam;
    private CinemachineVirtualCamera virtualRobotCamera;
    private RobotBuddyController robotController;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero;
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);

    // -------------------------------------------------------------------
    // Handle events

    // ALPHA
    public void OnBuildRobotAlpha()
    {
        robotAlphaObject = GameObject.Find("RobotBuddyAlpha");
        if (robotAlphaObject == null)
        {
            Debug.Log("[RobotManager]: robotAlpha not found in game");
            return;
        }
        hasBuiltAlpha = true;
    }

    public void OnRobotAlphaGrounded(bool state)
    {
        robotAlphaGrounded = state;
        // Debug.Log("[RobotManager]: robotAlphaGrounded: " + state);
    }

    public void OnRobotAlphaPositionUpdated(Vector2 position)
    {
        robotAlphaPosition = position;
        // Debug.Log("[GameManager]: robotAlphaPosition: " + position);
    }

    // BETA
    public void OnBuildRobotBeta()
    {
        robotBetaObject = GameObject.Find("RobotBuddyBeta");
        if (robotBetaObject == null)
        {
            Debug.Log("[RobotManager]: robotBetaObject not found in game");
            return;
        }
        hasBuiltBeta = true;
    }

    public void OnRobotBetaGrounded(bool state)
    {
        robotBetaGrounded = state;
        // Debug.Log("[RobotManager]: robotBetaGrounded: " + state);
    }

    public void OnRobotBetaPositionUpdated(Vector2 position)
    {
        robotBetaPosition = position;
        // Debug.Log("[GameManager]: robotBetaPosition: " + position);
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

    // ALPHA
    public bool GetRobotAlphaBuilt()
    {
        return hasBuiltAlpha;
    }

    public GameObject GetRobotAlphaObject()
    {
        return robotAlphaObject;
    }

    public bool GetRobotAlphaGrounded()
    {
        return robotAlphaGrounded;
    }

    public Vector2 GetRobotAlphaPosition()
    {
        return robotAlphaPosition;
    }

    // BETA
    public bool GetRobotBetaBuilt()
    {
        return hasBuiltBeta;
    }

    public GameObject GetRobotBetaObject()
    {
        return robotBetaObject;
    }

    public bool GetRobotBetaGrounded()
    {
        return robotBetaGrounded;
    }

    public Vector2 GetRobotBetaPosition()
    {
        return robotBetaPosition;
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
