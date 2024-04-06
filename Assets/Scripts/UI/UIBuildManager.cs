using UnityEngine;
using BuildingComponents;
using Unity.VisualScripting;
public class UIBuildManager : MonoBehaviour
{
    [SerializeField] GameObject buildChildOverlay;
    [SerializeField] private bool isOverlayActive;
    [SerializeField] public InventoryCheckEvent checkInventory;
    [SerializeField] private GameObject IndustryButtonOverlay;
    [SerializeField] private GameObject SuitButtonOverlay;
    [SerializeField] private GameObject RoboticsButtonOverlay;
    [SerializeField] private TechUpEvent techEvent;
    private GameObject currentOverlay;
    [SerializeField] private Sprite filledStar;
    private BuildingTierManager tierManager;
    private ObjectsCost techCost = new ObjectsCost(0,0,0,0,0,0,0,0,1);
    //<------------------------------------ <Industry Vars> ------------------------------------>
    //<---- <Extractor Text Fields> ---->
    [SerializeField] private GameObject Extractor_Tier1Star;
    [SerializeField] private GameObject Extractor_Tier2Star;
    [SerializeField] private GameObject Extractor_Tier3Star;
    //<---- <Commercial Extractor Text Fields> ---->
    [SerializeField] private GameObject CommercialExtractor_Tier1Star;
    [SerializeField] private GameObject CommercialExtractor_Tier2Star;
    [SerializeField] private GameObject CommercialExtractor_Tier3Star;
    //<---- <Industrial Extractor Text Fields> ---->
    [SerializeField] private GameObject IndustrialExtractor_Tier1Star;
    [SerializeField] private GameObject IndustrialExtractor_Tier2Star;
    [SerializeField] private GameObject IndustrialExtractor_Tier3Star;

    //<------------------------------------ <Suit Vars> --------------------------------------->

    //<---- <Exosuit Text Fields> ---->
    [SerializeField] private GameObject Exosuit_Tier1Star;
    [SerializeField] private GameObject Exosuit_Tier2Star;
    [SerializeField] private GameObject Exosuit_Tier3Star;
    //<---- <Jetpack Text Fields> ---->
    [SerializeField] private GameObject JetPack_Tier1Star;
    [SerializeField] private GameObject JetPack_Tier2Star;
    [SerializeField] private GameObject JetPack_Tier3Star;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private GameObject Cybernetics_Tier1Star;
    [SerializeField] private GameObject Cybernetics_Tier2Star;
    [SerializeField] private GameObject Cybernetics_Tier3Star;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    //<---- <RoboBuddy Text Fields> ---->
    [SerializeField] private GameObject RobotBuddy_Tier1Star;
    [SerializeField] private GameObject RobotBuddy_Tier2Star;
    [SerializeField] private GameObject RobotBuddy_Tier3Star;
    [SerializeField] private GameObject RobotBuddy_Tier4Star;
    //<---- <Satellite Text Fields> ---->
    [SerializeField] private GameObject Satellite_Tier1Star;
    [SerializeField] private GameObject Satellite_Tier2Star;
    [SerializeField] private GameObject Satellite_Tier3Star;
    //<---- <LaunchPad Text Fields> ---->
    [SerializeField] private GameObject LaunchPad_Tier1Star;
    [SerializeField] private GameObject LaunchPad_Tier2Star;
    [SerializeField] private GameObject LaunchPad_Tier3Star;
    [SerializeField] private GameObject LaunchPad_Tier4Star;
    
    //<------------------- <========<>========> ------------------->
    private void Start()
    {
        isOverlayActive = false;
        buildChildOverlay.SetActive(false);
        tierManager = new BuildingTierManager();
    }


    // -------------------------------------------------------------------
    // Handle events
    public bool IsActive(){
        return isOverlayActive;
    }
    public void OnPlayerBuildOverlay()
    {
        isOverlayActive = !isOverlayActive;
        buildChildOverlay.SetActive(isOverlayActive);

        currentOverlay = IndustryButtonOverlay;
        IndustryButtonOverlay.SetActive(true);
        SuitButtonOverlay.SetActive(false);
        RoboticsButtonOverlay.SetActive(false);
    }

    //  <Suit> | <Industry> | <Robotics>
    //Always Starts Industry, cylce left continuously
    public void OnPlayerBuildOverlayCycleLeft(){
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }
    //  <Suit> | <Industry> | <Robotics>
    public void OnPlayerBuildOverlayCycleRight(){
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }

    // -------------------------------------------------------------------

    public void UpdateAllTextObjects(packet.UpdateButtonCostTextPacket packet){

    }
    public void OnTechUpEvent(packet.TechUpPacket packet){
        tierManager.UpdateBuildingTier(packet.building, packet.TechToLevel);
        //changes the stars
        switch(packet.building){
            case(BuildingComponents.BuildingType.Extractor):
                if(packet.TechToLevel == 2){
                    Extractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Extractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.CommercialExtractor):
                if(packet.TechToLevel == 1){
                    CommercialExtractor_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    CommercialExtractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    CommercialExtractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.IndustrialExtractor):
                if(packet.TechToLevel == 1){
                    IndustrialExtractor_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    IndustrialExtractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    IndustrialExtractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Exosuit):
                if(packet.TechToLevel == 1){
                    Exosuit_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Exosuit_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Exosuit_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.JetPack):
                if(packet.TechToLevel == 1){
                    JetPack_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    JetPack_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    JetPack_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Cybernetics):
                if(packet.TechToLevel == 1){
                    Cybernetics_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Cybernetics_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Cybernetics_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.RobotBuddy):
                if(packet.TechToLevel == 1){
                    RobotBuddy_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    RobotBuddy_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    RobotBuddy_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 4){
                    RobotBuddy_Tier4Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Satellite):
                if(packet.TechToLevel == 1){
                    Satellite_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Satellite_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Satellite_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.LaunchPad):
                if(packet.TechToLevel == 1){
                    LaunchPad_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    LaunchPad_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    LaunchPad_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 4){
                    LaunchPad_Tier4Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
        }
    }

    //----------< UIBuild/TechUp Button Functions >-----------//
    public void OnTechQuery(BuildingType building){
        //I beilieve this function is depracated
        //techEvent.Raise(new packet.TechUpPacket(building, tierManager.GetTierOf(building)));
    }
    //<------------------------------------ <Industry Functions> ------------------------------------>
    public void TryBuildExtractor(){
        Extractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpExtractor(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.Extractor, techCost));
    }
    public void TryBuildCommercialExtractor(){
        CommercialExtractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpCommercialExtractor(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.CommercialExtractor, techCost));
    }
    public void TryBuildIndustrialExtractor(){
        IndustrialExtractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpIndustrialExtractor(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.IndustrialExtractor, techCost));
    }
    //<------------------------------------ <Suit Functions> ------------------------------------>
    public void TryBuildExosuit(){
        //Implementation needed
        Debug.Log("Implementation needed");
    }
    public void TryTechUpExosuit(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Exosuit, techCost));
    }
    public void TryBuildJetPack(){
        //Implementation needed
        Debug.Log("Implementation needed");
    }
    public void TryTechUpJetPack(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.JetPack, techCost));
    }
    public void TryBuildCybernetics(){
        //Implementation needed
        Debug.Log("Implementation needed");
    }
    public void TryTechUpCybernetics(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Cybernetics, techCost));
    }
    //<------------------------------------ <Robotics Functions> ------------------------------------>
    public void TryBuildRobotBuddy(){
        RobotBuddy tempRoboBuddy = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.RobotBuddy, tempRoboBuddy.GetCostDictionary()));
    }
    public void TryTechUpRobotBuddy(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.RobotBuddy, techCost));
    }
    public void TryBuildSatellite(){
        //Implementation needed
        Debug.Log("Implementation needed");
    }
    public void TryTechUpSatellite(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Satellite, techCost));
    }
    public void TryBuildLaunchPad(){
        LaunchPad tempLaunchPad = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.LaunchPad, tempLaunchPad.GetCostDictionary()
        ));
    }
    public void TryTechUpLaunchPad(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.LaunchPad, techCost));
    }
}
