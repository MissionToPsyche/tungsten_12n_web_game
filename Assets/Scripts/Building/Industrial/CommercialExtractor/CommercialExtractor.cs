public class CommercialExtractor : AbstractExtractor
{
    public CommercialExtractor(){
        buildingType = BuildingComponents.BuildingType.CommercialExtractor;
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("CommercialExtractor");
        SetVarsFromJsonData(thisObject.IntervalMine, thisObject.AmountToMine, thisObject.baseBreakChance, thisObject.percentAsteroidReach);
        thisCosts = InitObjCost(thisObject);
    }

}