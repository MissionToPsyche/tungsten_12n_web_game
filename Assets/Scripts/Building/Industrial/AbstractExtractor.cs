using UnityEngine;
using BuildingComponents;
using System.IO;
using Newtonsoft.Json;
public abstract class AbstractExtractor
{
    protected GameObject prefab;
    protected BuildingData buildingData;
    protected ObjectsCost thisCosts;
    protected float mineInterval;
    protected int amountToMine;
    protected float baseBreakChance;
    protected float AsteroidReach;
    protected int currentTier = 0;
    protected BuildingComponents.BuildingType buildingType;
    // Abstract method to get the cost dictionary
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
    protected void SetVarsFromJsonData(float mineInterval, int amountToMine, float baseBreakChance, float AsteroidReach){
        this.mineInterval = mineInterval;
        this.amountToMine = amountToMine;
        this.baseBreakChance = baseBreakChance;
        this.AsteroidReach = AsteroidReach;
    }

    public ObjectsCost GetCostDictionary(){
        return thisCosts;
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
    public float GetMineInterval(){
        return mineInterval;
    }
    public int GetAmountToMine(){
        return amountToMine;
    }
    public float GetBaseBreakChance(){
        return baseBreakChance;
    }
    public float GetAsteroidReach(){
        return AsteroidReach;
    }
}

