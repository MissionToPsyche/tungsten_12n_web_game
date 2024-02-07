using System.IO;
using Newtonsoft.Json;
using BuildingComponents;
using UnityEngine;
using TMPro;
using System;
public class BuildingDataLoader : MonoBehaviour
{
    private string filePath = "Assets/Resources/BuildingData.json"; // Adjust the path as per your project structure

    //<------------------------------------ <Industry Vars> ------------------------------------>
    //<---- <Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI ExtractorCostTexts;
    [SerializeField] private TextMeshProUGUI ExtractorHelpText;
    //<---- <Commercial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CommercialExtractorCostTexts;
    [SerializeField] private TextMeshProUGUI CommercialExtractorHelpText;
    //<---- <Industrial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI IndustrialExtractorCostTexts;
    [SerializeField] private TextMeshProUGUI IndustrialExtractorHelpText;

    //<------------------------------------ <Suit Vars> --------------------------------------->

    //<---- <Masterkey Text Fields> ---->
    [SerializeField] private TextMeshProUGUI MasterkeyCostTexts;
    [SerializeField] private TextMeshProUGUI MasterkeyHelpText;
    //<---- <Jetpack Text Fields> ---->
    [SerializeField] private TextMeshProUGUI JetpackCostTexts;
    [SerializeField] private TextMeshProUGUI JetpackHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CyberneticsCostTexts;
    [SerializeField] private TextMeshProUGUI CyberneticsHelpText;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    //<---- <RoboBuddy Text Fields> ---->
    [SerializeField] private TextMeshProUGUI RoboBuddyCostTexts;
    [SerializeField] private TextMeshProUGUI RoboBuddyHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI SatelliteCostTexts;
    [SerializeField] private TextMeshProUGUI SatelliteHelpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI AICostTexts;
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
                LoadIntoUI(item, ExtractorCostTexts, ExtractorHelpText);
            break;
            case "CommercialExtractor":
                LoadIntoUI(item, CommercialExtractorCostTexts, CommercialExtractorHelpText);
            break;
            case "IndustrialExtractor":
                LoadIntoUI(item, IndustrialExtractorCostTexts, IndustrialExtractorHelpText);
            break;
            case "Masterkey":
                LoadIntoUI(item, MasterkeyCostTexts, MasterkeyHelpText);
            break;
            case "Jetpack":
                LoadIntoUI(item, JetpackCostTexts, JetpackHelpText);
            break;
            case "Cybernetics":
                LoadIntoUI(item, CyberneticsCostTexts, CyberneticsHelpText);
            break;
            case "RoboBuddy":
                LoadIntoUI(item, RoboBuddyCostTexts, RoboBuddyHelpText);
            break;
            case "Satellite":
                LoadIntoUI(item, SatelliteCostTexts, SatelliteHelpText);
            break;
            case "AI":
                LoadIntoUI(item, AICostTexts, AIHelpText);
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
