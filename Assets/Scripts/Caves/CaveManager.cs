using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance { get; private set; }

    // [Header("Events")]

    // [Header("Mutable")]

    // [Header("ReadOnly")]

    // Not for display
    private AsyncOperation async;

    // -------------------------------------------------------------------
    // Handle events



    // -------------------------------------------------------------------
    // API

    public void LoadAsteroidScene(String caveScene)
    {
        StartCoroutine(UnloadCaveSceneAsync(caveScene));
    }

    public void LoadCaveScene(String caveScene)
    {
        StartCoroutine(LoadCaveSceneAsync(caveScene));
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator LoadCaveSceneAsync(String caveScene)
    {
        async = SceneManager.LoadSceneAsync(caveScene, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator UnloadCaveSceneAsync(String caveScene)
    {
        async = SceneManager.UnloadSceneAsync(caveScene);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
