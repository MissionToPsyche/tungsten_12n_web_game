using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveSceneManager : MonoBehaviour
{
    public static CaveSceneManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _ProgressBar;
    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName){
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do {

        }while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
    }
}
