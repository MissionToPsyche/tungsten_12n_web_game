using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    // Input
    [SerializeField] private InputReader inputReader;

    [SerializeField] GameObject buildOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive;

    private bool showInudstryButtons;
    private bool showSuitButton;
    private bool showSatelliteButtons;
    //<------------------- <Industry Vars> ------------------->
    private bool showingIndustryButtons;
    [SerializeField] private Button Build_Extractor_Button;
    [SerializeField] private Button Build_Commercial_Extractor_Button;
    [SerializeField] private Button Build_Industrial_Extractor_Button;

    //<------------------- <Suit Vars> ------------------->
    private bool showingSuitButtons;
    [SerializeField] private Button Build_MasterKey_Button;
    [SerializeField] private Button Build_ExoSuit_Button;
    [SerializeField] private Button Build_JetPack_Button;

    //<------------------- <Industry Vars> ------------------->
    private bool showingSatelliteButtons;
    [SerializeField] private Button Build_Satellite_Button;
    [SerializeField] private Button Build_Communication_Module_Button;
    [SerializeField] private Button Build_Commercial_Communication_Button;

    //<------------------- <========<>========> ------------------->
    public GameObject prefab;

    private void Start()
    {
        buildOverlay.SetActive(false);
        setIndustryVarsAndButtons(false);
        setSuitVarsAndButtons(false);
        setSatelliteVarsAndButton(false);
    }

    // -------------------------------------------------------------------

    void OnEnable()
    {
        // Subscribe to events
        inputReader.PlayerBuildOverlay += OnPlayerBuildOverlay;
        inputReader.PlayerBuildOverlayRight += OnPlayerBuildOverlayRight;
        inputReader.PlayerBuildOverlayLeft += OnPlayerBuildOverlayLeft;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        inputReader.PlayerBuildOverlay -= OnPlayerBuildOverlay;
        inputReader.PlayerBuildOverlayRight -= OnPlayerBuildOverlayRight;
        inputReader.PlayerBuildOverlayLeft -= OnPlayerBuildOverlayLeft;
    }

    // -------------------------------------------------------------------
    // Handle events

    private void OnPlayerBuildOverlay()
    {
        isOverlayActive = !isOverlayActive;
        buildOverlay.SetActive(isOverlayActive);
        setIndustryVarsAndButtons(true);
        setSuitVarsAndButtons(false);
        setSatelliteVarsAndButton(false);
    }

    private void OnPlayerBuildOverlayLeft(){
        if (!isOverlayActive) return;
        cycleMenusLeft();
    }

    private void OnPlayerBuildOverlayRight(){
        if (!isOverlayActive) return;
        cycleMenusRight();
    }

    // -------------------------------------------------------------------
    
    private void setIndustryVarsAndButtons(bool setTo){
        showingIndustryButtons = setTo;
        showInudstryButtons = setTo;
        Build_Extractor_Button.interactable = setTo;
        Build_Commercial_Extractor_Button.interactable = setTo;
        Build_Industrial_Extractor_Button.interactable = setTo;
        Build_Extractor_Button.gameObject.SetActive(setTo);
        Build_Commercial_Extractor_Button.gameObject.SetActive(setTo);
        Build_Industrial_Extractor_Button.gameObject.SetActive(setTo);
    }
    private void setSuitVarsAndButtons(bool setTo){
        showingSuitButtons = setTo;
        Build_MasterKey_Button.enabled = setTo;
        Build_ExoSuit_Button.enabled = setTo;
        Build_JetPack_Button.enabled = setTo;
        Build_MasterKey_Button.gameObject.SetActive(setTo);
        Build_ExoSuit_Button.gameObject.SetActive(setTo);
        Build_JetPack_Button.gameObject.SetActive(setTo);
    }
    private void setSatelliteVarsAndButton(bool setTo){
        showingSatelliteButtons = setTo;
        Build_Satellite_Button.enabled = setTo;
        Build_Communication_Module_Button.enabled = setTo;
        Build_Commercial_Communication_Button.enabled = setTo;
        Build_Satellite_Button.gameObject.SetActive(setTo);
        Build_Communication_Module_Button.gameObject.SetActive(setTo);
        Build_Commercial_Communication_Button.gameObject.SetActive(setTo);
    }

    // <suit> | <industry> | <satellite>
    private void cycleMenusLeft(){
        if(showingIndustryButtons){
            //go to suit
            setIndustryVarsAndButtons(false);
            setSuitVarsAndButtons(true);
        }else if(showingSuitButtons){
            //go to sat
            setSuitVarsAndButtons(false);
            setSatelliteVarsAndButton(true);
        }else if(showingSatelliteButtons){
            //go to industry
            setSatelliteVarsAndButton(false);
            setIndustryVarsAndButtons(true);
        }
    }

    private void cycleMenusRight(){
        if(showingIndustryButtons){
            //go to sat
            setIndustryVarsAndButtons(false);
            setSatelliteVarsAndButton(true);
        }else if(showingSuitButtons){
            //go to industry
            setSuitVarsAndButtons(false);
            setIndustryVarsAndButtons(true);
        }else if(showingSatelliteButtons){
            //go to suit
            setSatelliteVarsAndButton(false);
            setSuitVarsAndButtons(true);
        }
    }
    // -------------------------------------------------------------------

    public void SpawnNewEntity()
    {
        Vector3 screenPos = new Vector3(375, 285, 10f); 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
        buildOverlay.SetActive(false);
    }
}
