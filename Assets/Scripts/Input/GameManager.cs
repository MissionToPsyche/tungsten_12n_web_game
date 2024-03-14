using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    // 03.13.24
    void Start()
    {

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
