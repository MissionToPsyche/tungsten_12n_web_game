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
    private PlayerController playerController; 
    private CinemachineVirtualCamera virtualPlayerCamera;
    private Camera cam; 

    // 
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;   
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        playerInstance = GameObject.FindWithTag("Player");
        playerController = playerInstance.GetComponent<PlayerController>();  
        setScenePosition(); 

        virtualPlayerCamera = GameObject.FindWithTag("VirtualPlayerCamera").GetComponent<CinemachineVirtualCamera>(); 
        virtualPlayerCamera.Follow = playerInstance.transform; 
        virtualPlayerCamera.LookAt = playerInstance.transform;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = playerInstance.transform.position;
        cam.transform.rotation = Quaternion.Euler(0,0,0);
    }


    // set the players initial position and jump force
    private void setScenePosition() 
    {
        Transform defaultSpawn = GameObject.FindWithTag("Respawn").GetComponent<Transform>();
        String sceneName = SceneManager.GetActiveScene().name;

        // put the player but to their original position if they come back to the asteroid scene
        switch (sceneName)
        {
            case "AsteroidScene":
                if (this.lastCoordinates != Vector3.zero && this.lastRotation != Quaternion.Euler(0,0,0)) 
                {
                    this.playerInstance.transform.position = this.lastCoordinates; 
                    this.playerInstance.transform.rotation = this.lastRotation;
                    this.playerController.UpdateJumpForce(1.5f);
                }
                else 
                {
                    // put the player to the default spawn position in the scene
                    defaultSpawn = GameObject.FindWithTag("Respawn").GetComponent<Transform>();
                    this.playerInstance.transform.position = defaultSpawn.position;
                    this.playerInstance.transform.rotation = Quaternion.Euler(0,0,0);
                    this.playerController.UpdateJumpForce(1.5f);
                }
                break;

            default:
                // put the player to the default spawn position in the scene
                defaultSpawn = GameObject.FindWithTag("Respawn").GetComponent<Transform>();
                this.playerInstance.transform.position = defaultSpawn.position;
                this.playerInstance.transform.rotation = Quaternion.Euler(0,0,0);
                this.playerController.UpdateJumpForce(0.5f);
                break; 
        }
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
