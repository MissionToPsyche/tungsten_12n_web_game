using System.IO;
using BuildingComponents;
using Newtonsoft.Json;

public class BaseBuilding
{
    protected BuildingData buildingData;
    protected ObjectsCost thisCosts;
    protected int currentTier = 0;
    public ObjectsCost GetCostDictionary(){
        return thisCosts;
    }
    protected BuildingData LoadBuildingData()
    {
        string filePath = "Assets/Resources/BuildingData.json";
        string jsonData = File.ReadAllText(filePath);
        BuildingData buildingData = JsonConvert.DeserializeObject<BuildingData>(jsonData);

        return buildingData;
    }
    protected BuildingObject FindBuildingObjectByID(string id)
    {
        foreach (var buildingObject in buildingData.BuildingObject)
        {
            if (buildingObject.ID == id)
            {
                return buildingObject;
            }
        }
        return null;
    }

    protected ObjectsCost InitObjCost(BuildingObject thisObject){
        thisCosts = new ObjectsCost(
                thisObject.Costs["Iron"],
                thisObject.Costs["Nickel"],
                thisObject.Costs["Cobalt"],
                thisObject.Costs["Platinum"],
                thisObject.Costs["Gold"],
                thisObject.Costs["Technitium"],
                thisObject.Costs["Tungsten"],
                thisObject.Costs["Iridium"],
                0
            );
        return thisCosts;
    }

}