using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]

    [Header("ReadOnly")]
    [ReadOnly] public List<GameObject> Caves;
    [ReadOnly] public List<Transform> CaveSpawns;

    // Not for display
    private AsyncOperation async;

    // -------------------------------------------------------------------
    // Handle events


    // -------------------------------------------------------------------
    // API

    public Transform EnterCaveScene(string CaveScene)
    {
        int CaveIndex = int.Parse(ExtractPositionFromName(CaveScene)) - 1;

        Caves[CaveIndex].SetActive(true);

        return CaveSpawns[CaveIndex];
    }

    public void LeaveCaveScene(string CaveScene)
    {
        int CaveIndex = int.Parse(ExtractPositionFromName(CaveScene)) - 1;
        
        Caves[CaveIndex].SetActive(false);

    }

    // public void LoadAsteroidScene(String caveScene)
    // {
    //     StartCoroutine(UnloadCaveSceneAsync(caveScene));
    // }

    // -------------------------------------------------------------------
    // Class
    private void Start()
    {
        // ensure this object is not destroyed when transitioning between scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //
        string sceneName; 

        for (int i = 1; i < 11; i++)
        {
            sceneName = "Cave_" + i.ToString(); 
            LoadCaveScene(sceneName);
            //AssignCave(sceneName);
        }
    }

    private void LoadCaveScene(string caveScene)
    {
        StartCoroutine(LoadCaveSceneAsync(caveScene));
    }

    private void AssignCave(string caveScene)
    {
        List<GameObject> rootObject = new List<GameObject>(); 
        Scene scene = SceneManager.GetSceneByName(caveScene);
        scene.GetRootGameObjects( rootObject );
        Caves.Add(rootObject[0]);
    }

    private void DisableCave(string caveScene)
    {
        // disable the cave grid at the start of the game 
        for (int i = 0; i < Caves.Count; i++)
        {
            GameObject currentCave = Caves[i];
            if (currentCave.name == caveScene + "_Grid")
            {   
                Transform caveSpawn = currentCave.transform.GetChild(0).GameObject().transform;
                CaveSpawns.Add(caveSpawn);
                currentCave.SetActive(false);
            }
        }
    }
    
    private IEnumerator LoadCaveSceneAsync(string caveScene)
    {
        // additively load the cave scene
        async = SceneManager.LoadSceneAsync(caveScene, LoadSceneMode.Additive);
        
        // save the players last known coordinates

        // wait to load the scene until it is done
        while (!async.isDone)
        {
            yield return null;
        }
        AssignCave(caveScene);
        DisableCave(caveScene);
        yield return new WaitForSeconds(10);
    }

    private string ExtractPositionFromName(string name)
    {
        int underscoreIndex = name.IndexOf('_');
        if (underscoreIndex != -1 && underscoreIndex < name.Length - 1)
        {
            // Return the substring starting just after the underscore
            //Debug.Log(name.Substring(underscoreIndex + 1));
            return name.Substring(underscoreIndex + 1);
        }
        return null; // Return null if no underscore is found or it's at the end of the string
    }
}
