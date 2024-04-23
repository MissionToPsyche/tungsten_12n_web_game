
public class Exosuit : BaseBuilding{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.Exosuit;

    // Abstract method to get the cost dictionary
    public Exosuit(){
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("Exosuit");
        thisCosts = InitObjCost(thisObject);
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
}