
public class JetPack : BaseBuilding{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.JetPack;

    // Abstract method to get the cost dictionary
    public JetPack(){
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("JetPack");
        thisCosts = InitObjCost(thisObject);
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
}