using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private CanvasGroup GeneralOverlay;
    [SerializeField] private CanvasGroup HelpOverlay;
    [SerializeField] private GameObject BuildOverlay;
    [SerializeField] private CanvasGroup InventoryOverlay;
    [SerializeField] private CanvasGroup ControlOverlay;
    [SerializeField] private GameObject SideOverlay;

    // -------------------------------------------------------------------
    // Handle events
    public void ActivateGeneralOverlay()
    {
        TurnOnOverlay("General");
        TurnOffOverlay("Inventory");
        TurnOffOverlay("Side");
        TurnOffOverlay("Help");
        TurnOffOverlay("Build");
        TurnOffOverlay("Control");
    }

    public void ShortCutControlOverlay()
    {
        TurnOnOverlay("Control");
        TurnOffOverlay("Inventory");
        TurnOffOverlay("Side");
    }

    // -------------------------------------------------------------------
    // API


    // -------------------------------------------------------------------
    // Class
    public void Start()
    {
        TurnOnOverlay("Help");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReturnToGame(String OverlayName)
    {
        TurnOffOverlay(OverlayName);
        TurnOffOverlay("General");
        TurnOnOverlay("Inventory");
        TurnOnOverlay("Side");
        TurnOnOverlay("Build");
    }
    
    public void ReturnToMenu(String OverlayName)
    {
        TurnOffOverlay(OverlayName);
        TurnOnOverlay("General");
        TurnOffOverlay("Inventory");
        TurnOffOverlay("Side");
    }
    public void TurnOnOverlay(String ButtonName)
    {
        switch (ButtonName) 
        {
            case "General":
                GeneralOverlay.alpha = 1;
                GeneralOverlay.blocksRaycasts = true;
                break; 
            case "Build":
                BuildOverlay.SetActive(true); 
                break;
            case "Help":
                HelpOverlay.alpha = 1; 
                HelpOverlay.blocksRaycasts = true; 
                TurnOnOverlay("Inventory");
                TurnOnOverlay("Side");
                break;
            case "Inventory":
                InventoryOverlay.alpha = 1;
                InventoryOverlay.blocksRaycasts = true;
                break;
            case "Control":
                ControlOverlay.alpha = 1; 
                ControlOverlay.blocksRaycasts = true; 
                break;
            case "Side":
                SideOverlay.SetActive(true);
                break;
        }
    }

    public void TurnOffOverlay(String OverlayName)
    {
        switch (OverlayName)
        {
            case "General":
                GeneralOverlay.alpha = 0;
                GeneralOverlay.blocksRaycasts = false; 
                break;
            case "Help":
                HelpOverlay.alpha = 0;
                HelpOverlay.blocksRaycasts = false; 
                break;
            case "Build":
                BuildOverlay.SetActive(false); 
                break;
            case "Inventory":
                InventoryOverlay.alpha = 0;
                InventoryOverlay.blocksRaycasts = false; 
                break;
            case "Control":
                ControlOverlay.alpha = 0;
                ControlOverlay.blocksRaycasts = false; 
                break;
            case "Side":
                SideOverlay.SetActive(false); 
                break;
        }
    }
}
