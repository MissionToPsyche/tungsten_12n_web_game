public class Extractor : AbstractExtractor
{
    public Extractor(){
        buildingType = BuildingComponents.BuildingType.Extractor;
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("Extractor");
        SetVarsFromJsonData(thisObject.IntervalMine, thisObject.AmountToMine, thisObject.baseBreakChance, thisObject.percentAsteroidReach);
        thisCosts = InitObjCost(thisObject);
    }


}