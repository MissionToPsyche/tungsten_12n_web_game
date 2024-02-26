public class IndustrialExtractor : AbstractExtractor
{
    public IndustrialExtractor(){
        buildingType = BuildingComponents.BuildingType.IndustrialExtractor;
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("IndustrialExtractor");
        SetVarsFromJsonData(thisObject.IntervalMine, thisObject.AmountToMine, thisObject.baseBreakChance, thisObject.percentAsteroidReach);
        thisCosts = InitObjCost(thisObject);
    }

}