using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BuildingComponents;
public class InventoryManager : MonoBehaviour
{
    Inventory myInventory;
    InventoryUIManager inventoryUI;
    [SerializeField] public BuildObjEvent buildObjEvent;
    void Awake(){
        //init resource dictionary and add their starting resources
        myInventory = new Inventory(35, 35, 15, 0, 0, 0, 0, 0);
        inventoryUI = GetComponent<InventoryUIManager>();
        inventoryUI.UpdateInventoryFromDictionary(myInventory.GetInvDictionary());
    }
    void Update(){
        //logic to update the ui to be red in real time when not enough resources
    }
    public void OnCheckInventoryEvent(packet.CheckInventoryPacket packet){
        if(CheckAvailResources(packet.objCost) == true){
            myInventory.PayForObjectWithObjCost(packet.objCost);
            inventoryUI.UpdateInventoryFromDictionary(GetMyInventory());
            buildObjEvent.Raise(packet.building);
        }
    }
    public Dictionary<ResourceType, int> GetMyInventory(){
        return myInventory.GetInvDictionary();
    }

    private bool CheckAvailResources(ObjectsCost costDictionary){
        return myInventory.CheckCost(costDictionary);
    }

    public void OnMineEvent(packet.MiningPacket packet){
        myInventory.AddResource(packet.resourceToChange, packet.amountToChange);
        inventoryUI.UpdateInventoryFromDictionary(myInventory.GetInvDictionary());
    }

}