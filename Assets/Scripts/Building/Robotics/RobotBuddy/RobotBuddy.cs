using BuildingComponents;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class RobotBuddy
{
    private BuildingData buildingData;
    private ObjectsCost thisCosts;
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.RobotBuddy;

    //Gameplay related member vars
    private float currentCharge = 600;
    //Changing the maxCharge value requires you to change the maxCharge value in RobotBuddyUIManager as well
    private readonly float maxCharge = 600;

    // Abstract method to get the cost dictionary
    public RobotBuddy(){
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("RobotBuddy");
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
    public float ReduceCharge(float reduceBy){
        currentCharge -= reduceBy;
        return currentCharge;
    }

    public void GiveFullCharge(){
        currentCharge = maxCharge;
    }

    public float GetCurrentCharge(){
        return currentCharge;
    }
}
