using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero; 
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);
    private GameObject playerInstance; 
    private CinemachineVirtualCamera virtualPlayerCamera;
    private Camera cam; 

    // 
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;   
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        Debug.Log($"Last coordinates: {this.lastCoordinates}");
        Debug.Log($"Last rotation: {this.lastRotation}");
        playerInstance = GameObject.FindWithTag("Player");
        setScenePosition(); 

        virtualPlayerCamera = GameObject.FindWithTag("VirtualPlayerCamera").GetComponent<CinemachineVirtualCamera>(); 
        virtualPlayerCamera.Follow = playerInstance.transform; 
        virtualPlayerCamera.LookAt = playerInstance.transform;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = playerInstance.transform.position;
        cam.transform.rotation = Quaternion.Euler(0,0,0);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // // Singleton method
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

    // set the players initial position
    private void setScenePosition() 
    {
        // put the player but to their original position if they come back to the asteroid scene
        if (SceneManager.GetActiveScene().name == "AsteroidScene" && this.lastCoordinates != Vector3.zero && this.lastRotation != Quaternion.Euler(0,0,0)) 
        {  
            this.playerInstance.transform.position = this.lastCoordinates; 
            this.playerInstance.transform.rotation = this.lastRotation;
            return; 
        }
        
        // put the player to the default spawn position in the scene
        Transform defaultSpawn = GameObject.FindWithTag("Respawn").GetComponent<Transform>();
        this.playerInstance.transform.position = defaultSpawn.position;
        this.playerInstance.transform.rotation = Quaternion.Euler(0,0,0);
    }

    // set the players last known coordinates
    public void setLastPosition()
    {  
        this.lastCoordinates = playerInstance.transform.position;
        this.lastRotation = playerInstance.transform.rotation;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
