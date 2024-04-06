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
        SceneManager.sceneUnloaded += OnSceneUnloaded;    
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        sceneChange(scene);
    }

    void OnSceneUnloaded(Scene scene)
    {
        sceneChange(SceneManager.GetSceneByName("AsteroidScene"));
    }

    private void sceneChange(Scene scene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
        playerInstance = GameObject.FindWithTag("Player");
        playerController = playerInstance.GetComponent<PlayerController>();  
        setScenePosition(scene.name); 

        virtualPlayerCamera = GameObject.FindWithTag("VirtualPlayerCamera").GetComponent<CinemachineVirtualCamera>(); 
        virtualPlayerCamera.Follow = playerInstance.transform; 
        virtualPlayerCamera.LookAt = playerInstance.transform;

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
    public void setScenePosition(String sceneName) 
    {
        Transform defaultSpawn = findCorrectSpawn(sceneName); 

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
                    this.playerInstance.transform.position = defaultSpawn.position;
                    this.playerInstance.transform.rotation = Quaternion.Euler(0,0,0);
                    this.playerController.UpdateJumpForce(1.5f);
                }
                break;

            default:
                // put the player to the default spawn position in the scene
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
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
