using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using myEnums;
public class InventoryManager : MonoBehaviour
{
    Inventory myInventory;
    [SerializeField] InventoryUIManager invUI;
    [SerializeField] public BuildObjEvent buildObjEvent;
    void Awake(){
        //init resource dictionary and add their starting resources
        myInventory = new Inventory(35, 35, 15, 0, 0, 0, 0, 0);
        invUI.UpdateInventoryFromDictionary(myInventory.GetInvDictionary());
    }

    public Inventory GetMyInventory(){
        return myInventory;
    }

    private bool CheckAvailResources(ObjectsCost costDictionary){
        return myInventory.CheckCost(costDictionary);
    }

    public void OnMineEvent(packet.MiningPacket packet){
        myInventory.AddResource(packet.resourceToChange, packet.amountToChange);
        invUI.UpdateInventoryFromDictionary(myInventory.GetInvDictionary());
    }

    public void OnCheckInventoryEvent(packet.CheckInventoryPacket packet){
        if(CheckAvailResources(packet.objCost) == true){
            myInventory.PayForObjectWithObjCost(packet.objCost);
            buildObjEvent.Raise(packet.building);
        }
    }
}