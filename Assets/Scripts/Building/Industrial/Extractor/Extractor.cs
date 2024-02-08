using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Extractor : AbstractExtractor
{
    public Extractor(){
        buildingType = BuildingComponents.BuildingType.Extractor;
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("Extractor");
        SetVarsFromJsonData(thisObject.IntervalMine, thisObject.AmountToMine, thisObject.baseBreakChance);
        thisCosts = InitObjCost(thisObject);
    }

}