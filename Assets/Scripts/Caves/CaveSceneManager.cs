using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveSceneManager : MonoBehaviour
{
    AsyncOperation async;
    public void loadAsteroidScene(String caveScene)
    {
        StartCoroutine(UnloadCaveSceneAsync(caveScene));
    }
    public void loadCaveScene(String caveScene)
    {
        StartCoroutine(LoadCaveSceneAsync(caveScene));
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
