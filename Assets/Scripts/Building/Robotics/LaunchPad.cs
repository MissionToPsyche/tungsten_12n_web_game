using UnityEngine;
using BuildingComponents;
using System.IO;
using Newtonsoft.Json;

public class LaunchPad{
    private BuildingData buildingData;
    private ObjectsCost thisCosts;
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.LaunchPad;

    //Building focused vars
    private bool hasBuiltEngines = false;
    private bool hasBuiltChasis = false;
    private bool hasBuiltCockpit = false;
    private bool hasBuiltCompWire = false;

    // Abstract method to get the cost dictionary
    public LaunchPad(){
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("LaunchPad");
        thisCosts = InitObjCost(thisObject);
    }
    public BuildingData LoadBuildingData()
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
    public ObjectsCost GetCostDictionary(){
        return thisCosts;
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
    public bool isEngineBuilt(){
        return hasBuiltEngines;
    }
    public bool isChasisBuilt(){
        return hasBuiltChasis;
    }
    public bool isCockpitBuilt(){
        return hasBuiltCockpit;
    }
    public bool isCompWireBuilt(){
        return hasBuiltCompWire;
    }

}