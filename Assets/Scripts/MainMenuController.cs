using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup HelpOption;
    [SerializeField] private CanvasGroup AboutOption;
    [SerializeField] private CanvasGroup CharacterSelection; 
    [SerializeField] private CanvasGroup DisclaimerSection;
    [SerializeField] private CanvasGroup ControlSection;
    [SerializeField] private SpriteRenderer CharacterSprite;
    [SerializeField] private Slider LoadingSlider;
     
 
    public void Start()
    {
        SoundFXManager.Instance.PlaySound(SFX.Music.Asteroid.MainMenu, this.gameObject.transform, 0.5f, 1f);
        CharacterSprite.enabled = false;
    }

    public void PlayGame()
    {
        SoundFXManager.Instance.StopSoundsOfType(typeof(SFX.Music.Asteroid));

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

    public void MainMenuContinue()
    {
        CharacterSelection.alpha = 1; 
        CharacterSelection.blocksRaycasts = true;
        CharacterSprite.enabled = true;
    }

    public void CharacterContinue()
    {
        CharacterSelection.alpha = 0; 
        CharacterSelection.blocksRaycasts = false; 
        DisclaimerSection.alpha = 1; 
        DisclaimerSection.blocksRaycasts = true; 
        CharacterSprite.enabled = false;
    }

        public void DisclaimerContinue()
    {
        DisclaimerSection.alpha = 0;
        DisclaimerSection.blocksRaycasts = false;
        ControlSection.alpha = 1; 
        ControlSection.blocksRaycasts = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
