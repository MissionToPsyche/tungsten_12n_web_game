using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] private GameObject pauseMenu;

    void Start()
    {

    }

    // -------------------------------------------------------------------

    private void OnEnable()
    {
        inputReader.PauseGame += OnPauseGame;
        inputReader.ResumeGame += OnResumeGame;
    }

    private void OnDisable()
    {
        inputReader.PauseGame -= OnPauseGame;
        inputReader.ResumeGame -= OnResumeGame;
    }

    // -------------------------------------------------------------------
    // Handle events

    private void OnPauseGame()
    {
        pauseMenu.SetActive(true);
    }

    private void OnResumeGame()
    {
        pauseMenu.SetActive(false);
    }
}
