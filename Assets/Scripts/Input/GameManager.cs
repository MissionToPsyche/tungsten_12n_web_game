using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

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
