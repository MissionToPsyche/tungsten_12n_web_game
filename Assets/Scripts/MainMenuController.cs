using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup HelpOption;
    public CanvasGroup AboutOption;

    public void Start()
    {
        // SFX.Packet packet = new(SFX.Player.Jump, this.transform, 1f);
        // soundFXEvent.Raise(packet);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Help()
    {
        AboutOption.alpha = 0;
        AboutOption.blocksRaycasts = false;
        HelpOption.alpha = 1;
        HelpOption.blocksRaycasts = true;
    }

    public void About() {
        HelpOption.alpha = 0;
        HelpOption.blocksRaycasts = false;
        AboutOption.alpha = 1;
        AboutOption.blocksRaycasts = true;
    }

    public void Back()
    {
        HelpOption.alpha = 0;
        HelpOption.blocksRaycasts = false;
        AboutOption.alpha = 0;
        AboutOption.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
