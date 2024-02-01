using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIManager : MonoBehaviour
{
    [SerializeField] GameObject buildChildOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive;

    // Input
    [SerializeField] private InputReader inputReader;
    private BuildManager buildManager;
    [SerializeField] public InventoryCheckEvent checkInventory;
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
    private void Start()
    {
        buildChildOverlay.SetActive(false);

    }

    // -------------------------------------------------------------------


    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerBuildOverlay()
    {
        isOverlayActive = !isOverlayActive;
        buildChildOverlay.SetActive(isOverlayActive);
    }

    // -------------------------------------------------------------------

    public void TryBuildExtractor(){
        Extractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(this.gameObject, newExtractor.GetBuildingType(), newExtractor.getCostDictionary()));
    }
}
