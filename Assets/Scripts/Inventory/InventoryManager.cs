using System.Collections.Generic;
using UnityEngine;
using BuildingComponents;
using packet;
public class InventoryManager : MonoBehaviour
{
    Inventory currentInventory;
    InventoryUIManager inventoryUI;
    [SerializeField] public BuildObjEvent buildObjEvent;
    [SerializeField] public TechUpEvent techUpEvent;
    [SerializeField] public TechUpEvent techQueryResponse;
    [SerializeField] public UpdateButtonCostTextEvent updateBuildButtonsEvent;
    void Start(){
        //init resource dictionary and add their starting resources
        currentInventory = new Inventory(35, 35, 15, 0, 0, 0, 0, 0, 20);
        inventoryUI = GetComponent<InventoryUIManager>();
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }
    // void Update(){
    //      Still unsure on how to update BuildUI, potentially just have a text at the bottom of card
    //      that says not enough resources
    //     updateBuildButtonsEvent.Raise(new packet.UpdateButtonCostTextPacket(currentInventory.GetInvDictionary()));
    // }
    public void OnCheckInventoryEvent(packet.CheckInventoryPacket packet){
        BuildingType building = packet.building;
        if(CheckAvailResources(packet.objCost) == true){
            if(packet.objCost.hasTechCost() == false){
                if(packet.building == BuildingType.CommercialExtractor || packet.building == BuildingType.CommercialExtractor){
                    if(currentInventory.GetBuildingTechLevel(packet.building) > 0){
                        buildObjEvent.Raise(building);
                        return;
                    }else return;
                }
                PayForObject(packet.objCost);
                buildObjEvent.Raise(building);
            }else{
                CheckInvTechCase(packet);
            }
        }
    }

    private void CheckInvTechCase(packet.CheckInventoryPacket packet){

        int newLevel = currentInventory.GetBuildingTechLevel(packet.building) + 1;
        //Only two buildings with 4 levels, using 0 indexing
        if(packet.building == BuildingType.LaunchPad || packet.building == BuildingType.RobotBuddy){
            if(newLevel <= 4){
                PayForObject(packet.objCost);
                currentInventory.TechUpBuilding(packet.building, newLevel);
                techUpEvent.Raise(new TechUpPacket(packet.building, newLevel));
            }else{
                return;
            }
        //Every other building has 3 leves, using 0 indexing
        }else if(newLevel <= 3){
            PayForObject(packet.objCost);
            currentInventory.TechUpBuilding(packet.building, newLevel);
            techUpEvent.Raise(new TechUpPacket(packet.building, newLevel));
            return;
        }
    }
    public Dictionary<ResourceType, int> GetCurrentInventory(){
        return currentInventory.GetInvDictionary();
    }
    public void PayForObject(ObjectsCost cost){
        currentInventory.PayForObjectWithObjCost(cost);
        inventoryUI.UpdateInventoryFromDictionary(GetCurrentInventory());
    }
    private bool CheckAvailResources(ObjectsCost costDictionary){
        return currentInventory.CheckCost(costDictionary);
    }

    public void OnMineEvent(packet.MiningPacket packet){
        currentInventory.AddResource(packet.resourceToChange, packet.amountToChange);
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }
    public void OnTechQuery(BuildingType buildingType){
        techQueryResponse.Raise(new TechUpPacket(buildingType, currentInventory.GetBuildingTechLevel(buildingType)));
    }

    public void OnDiscoverGeoPhenom(){

    }
}