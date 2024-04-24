using BuildingComponents;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public class Satellite : BaseBuilding
{
    public string satelliteName = "";
    private ObjectsCost thisCosts;
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.Satellite;

    // Abstract method to get the cost dictionary
    public Satellite()
    {
        buildingData = LoadBuildingData("Satellite");
    }

    public ObjectsCost GetCostDictionary()
    {
        return thisCosts;
    }
    public BuildingComponents.BuildingType GetBuildingType()
    {
        return buildingType;
    }
}
