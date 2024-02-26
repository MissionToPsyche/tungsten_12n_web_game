using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BuildingComponents;
public class InventoryManager : MonoBehaviour
{
    Inventory currentInventory;
    InventoryUIManager inventoryUI;
    [SerializeField] public BuildObjEvent buildObjEvent;
    [SerializeField] public UpdateButtonCostTextEvent updateBuildButtonsEvent;
    void Start(){
        //init resource dictionary and add their starting resources
        currentInventory = new Inventory(35, 35, 15, 0, 0, 0, 0, 0);
        inventoryUI = GetComponent<InventoryUIManager>();
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }
    // void Update(){
    //      Still unsure on how to update BuildUI, potentially just have a text at the bottom of card
    //      that says not enough resources
    //     updateBuildButtonsEvent.Raise(new packet.UpdateButtonCostTextPacket(currentInventory.GetInvDictionary()));
    // }
    public void OnCheckInventoryEvent(packet.CheckInventoryPacket packet){
        if(CheckAvailResources(packet.objCost) == true){
            currentInventory.PayForObjectWithObjCost(packet.objCost);
            inventoryUI.UpdateInventoryFromDictionary(GetcurrentInventory());
            buildObjEvent.Raise(packet.building);
        }
    }
    public Dictionary<ResourceType, int> GetcurrentInventory(){
        return currentInventory.GetInvDictionary();
    }

    private bool CheckAvailResources(ObjectsCost costDictionary){
        return currentInventory.CheckCost(costDictionary);
    }

    public void OnMineEvent(packet.MiningPacket packet){
        currentInventory.AddResource(packet.resourceToChange, packet.amountToChange);
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }

}