using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private static GameManager instance; 
    

    // 03.13.24
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



    // -------------------------------------------------------------------

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    // -------------------------------------------------------------------
    // Handle events

    public void OnGamePause()
    {
        pauseMenu.SetActive(true);
    }

    public void OnGameResume()
    {
        pauseMenu.SetActive(false);
    }
}
