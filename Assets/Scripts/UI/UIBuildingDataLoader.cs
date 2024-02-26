using System.IO;
using Newtonsoft.Json;
using BuildingComponents;
using UnityEngine;
using TMPro;
using System;
public class UIBuildingDataLoader : MonoBehaviour
{
    //This class is responsible for loading in the building data into the UI, 
    //as well as updating the text to red or green based on whether or not the object is buyable
    private string filePath = "Assets/Resources/BuildingData.json"; // Adjust the path as per your project structure

    //<------------------------------------ <Industry Vars> ------------------------------------>
    //<---- <Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI ExtractorCostText;
    [SerializeField] private TextMeshProUGUI ExtractorHelpText;
    //<---- <Commercial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CommercialExtractorCostText;
    [SerializeField] private TextMeshProUGUI CommercialExtractorHelpText;
    //<---- <Industrial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI IndustrialExtractorCostText;
    [SerializeField] private TextMeshProUGUI IndustrialExtractorHelpText;

    //<------------------------------------ <Suit Vars> --------------------------------------->

    //<---- <Masterkey Text Fields> ---->
    [SerializeField] private TextMeshProUGUI MasterkeyCostText;
    [SerializeField] private TextMeshProUGUI MasterkeyHelpText;
    //<---- <Jetpack Text Fields> ---->
    [SerializeField] private TextMeshProUGUI JetpackCostText;
    [SerializeField] private TextMeshProUGUI JetpackHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CyberneticsCostText;
    [SerializeField] private TextMeshProUGUI CyberneticsHelpText;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    //<---- <RoboBuddy Text Fields> ---->
    [SerializeField] private TextMeshProUGUI RoboBuddyCostText;
    [SerializeField] private TextMeshProUGUI RoboBuddyHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI SatelliteCostText;
    [SerializeField] private TextMeshProUGUI SatelliteHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI AICostText;
    [SerializeField] private TextMeshProUGUI AIHelpText;
    //Json Vars
    //-----------------------------------------------------------------
    BuildingData buildingData;
    private void Start()
    {
        buildingData = LoadBuildingData();
        LoadBuildingUI(buildingData);
    }
    public BuildingData LoadBuildingData()
    {
        string jsonData = File.ReadAllText(filePath);
        BuildingData buildingData = JsonConvert.DeserializeObject<BuildingData>(jsonData);

        return buildingData;
    }
    private void LoadBuildingUI(BuildingData data){
            foreach (var buildingObject in data.BuildingObject)
            {
                SetUIBuildingItem(buildingObject);
            }
    }
    private void SetUIBuildingItem(BuildingObject item){
        string itemID = item.ID;
        
        switch(itemID){
            case "Extractor":
                LoadIntoUI(item, ExtractorCostText, ExtractorHelpText);
            break;
            case "CommercialExtractor":
                LoadIntoUI(item, CommercialExtractorCostText, CommercialExtractorHelpText);
            break;
            case "IndustrialExtractor":
                LoadIntoUI(item, IndustrialExtractorCostText, IndustrialExtractorHelpText);
            break;
            case "Masterkey":
                LoadIntoUI(item, MasterkeyCostText, MasterkeyHelpText);
            break;
            case "Jetpack":
                LoadIntoUI(item, JetpackCostText, JetpackHelpText);
            break;
            case "Cybernetics":
                LoadIntoUI(item, CyberneticsCostText, CyberneticsHelpText);
            break;
            case "RoboBuddy":
                LoadIntoUI(item, RoboBuddyCostText, RoboBuddyHelpText);
            break;
            case "Satellite":
                LoadIntoUI(item, SatelliteCostText, SatelliteHelpText);
            break;
            case "AI":
                LoadIntoUI(item, AICostText, AIHelpText);
            break;
        }
    }
    private void LoadIntoUI(BuildingObject item, TextMeshProUGUI costText, TextMeshProUGUI helpText){
        costText.text = "";
        foreach(var costNode in item.Costs){
            if (Convert.ToInt32(costNode.Value) > 0)
                costText.text += $"{costNode.Key}: {costNode.Value}\n";
        }
        helpText.text = item.ItemDescription;
    }


    
}
