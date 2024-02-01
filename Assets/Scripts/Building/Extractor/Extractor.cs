using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Extractor
{
    GameObject prefab;
    static InventoryTypes.BuildingType buildingType = InventoryTypes.BuildingType.Extractor;
    static int ironCost = 15;
    static int nickelCost = 15;
    static int silverCost = 5;
    static ObjectsCost thisCosts = new ObjectsCost(ironCost, nickelCost, silverCost, 0, 0, 0, 0, 0);

    public ObjectsCost getCostDictionary(){
        return thisCosts;
    }

    public InventoryTypes.BuildingType GetBuildingType(){
        return buildingType;
    }

}