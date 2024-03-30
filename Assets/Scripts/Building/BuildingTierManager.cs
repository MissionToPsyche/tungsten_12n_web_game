using System.Collections.Generic;
using BuildingComponents;

public class BuildingTierManager
{
    Dictionary<BuildingType, int> buildingTiers = new Dictionary<BuildingType, int>();
    public BuildingTierManager(){
        buildingTiers.Add(BuildingType.Extractor, 1);
        buildingTiers.Add(BuildingType.CommercialExtractor, 0);
        buildingTiers.Add(BuildingType.IndustrialExtractor, 0);
        buildingTiers.Add(BuildingType.Exosuit, 0);
        buildingTiers.Add(BuildingType.JetPack, 0);
        buildingTiers.Add(BuildingType.Cybernetics, 0);
        buildingTiers.Add(BuildingType.RobotBuddy, 0);
        buildingTiers.Add(BuildingType.Satellite, 0);
        buildingTiers.Add(BuildingType.LaunchPad, 0);
        
    }

    public void UpdateBuildingTier(BuildingType building, int newLevel){
        buildingTiers[building] = newLevel;
    }

    public int GetTierOf(BuildingType building){
        return buildingTiers[building];
    }
}