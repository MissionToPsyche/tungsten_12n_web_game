using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using TMPro;
using BuildingComponents;
public class UIBuildManager : MonoBehaviour
{
    [SerializeField] GameObject buildChildOverlay;
    [SerializeField] private bool isOverlayActive;
    [SerializeField] public InventoryCheckEvent checkInventory;
    [SerializeField] private GameObject IndustryButtonOverlay;
    [SerializeField] private GameObject SuitButtonOverlay;
    [SerializeField] private GameObject RoboticsButtonOverlay;
    private GameObject currentOverlay;
    
    //<------------------- <========<>========> ------------------->
    private void Start()
    {
        isOverlayActive = false;
        buildChildOverlay.SetActive(false);
    }


    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerBuildOverlay()
    {
        isOverlayActive = !isOverlayActive;
        buildChildOverlay.SetActive(isOverlayActive);

        currentOverlay = IndustryButtonOverlay;
        IndustryButtonOverlay.SetActive(true);
        SuitButtonOverlay.SetActive(false);
        RoboticsButtonOverlay.SetActive(false);
    }

    //  <Suit> | <Industry> | <Robotics>
    //Always Starts Industry, cylce left continuously
    public void OnPlayerBuildOverlayCycleLeft(){
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }
    //  <Suit> | <Industry> | <Robotics>
    public void OnPlayerBuildOverlayCycleRight(){
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }

    // -------------------------------------------------------------------

    public void UpdateAllTextObjects(packet.UpdateButtonCostTextPacket packet){

    }
    public void TryBuildExtractor(){
        Extractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
}
