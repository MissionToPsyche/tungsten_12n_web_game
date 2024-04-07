using UnityEngine;

public class BuildManager : MonoBehaviour
{

    //<------------------------------------ <Industry Vars> ------------------------------------>
    [SerializeField] public GameObject ExtractorPrefab;
    [SerializeField] public GameObject CommercialExtractorPrefab;
    [SerializeField] public GameObject IndustrialExtractorPrefab;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    [SerializeField] public GameObject RobotBuddyAlphaObject;
    private bool hasBuiltRobotBuddyAlpha = false;
    [SerializeField] public GameObject RobotBuddyBetaObject;
    private bool hasBuiltRobotBuddyBeta = false;
    [SerializeField] public BuildObjEvent alertPlayerController;
    [SerializeField] public GameObject LaunchPadPrefab;
    private bool hasBuiltLaunchPad = false;
    public VoidEvent satelliteSpawnTrigged;

    private UIBuildManager buildUI;

    void Awake()
    {
        buildUI = GetComponent<UIBuildManager>();

        if (satelliteSpawnTrigged == null)
        {
            satelliteSpawnTrigged = ScriptableObject.CreateInstance<VoidEvent>();
        }
    }
    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        //Turns off Build overlay after an obj has been bought
        if(buildUI.IsActive() == true){
            buildUI.OnPlayerBuildOverlay();
        }

        switch(type){
            case BuildingComponents.BuildingType.Extractor:
                SpawnNewEntity(ExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.CommercialExtractor:
                SpawnNewEntity(CommercialExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.IndustrialExtractor:
                SpawnNewEntity(IndustrialExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.Exosuit:

                return;
            case BuildingComponents.BuildingType.JetPack:

                return;
            case BuildingComponents.BuildingType.Cybernetics:

                return;
            case BuildingComponents.BuildingType.RobotBuddy:
                if(hasBuiltRobotBuddyAlpha == false){
                    TeleportRobotBuddy(RobotBuddyAlphaObject);
                    hasBuiltRobotBuddyAlpha = true;
                    alertPlayerController.Raise(BuildingComponents.BuildingType.RobotBuddyAlpha);
                }else if(hasBuiltRobotBuddyBeta == false){
                    TeleportRobotBuddy(RobotBuddyBetaObject);
                    hasBuiltRobotBuddyBeta = true;
                    alertPlayerController.Raise(BuildingComponents.BuildingType.RobotBuddyBeta);
                }
                return;
            case BuildingComponents.BuildingType.Satellite:
                Debug.Log("[GameManager]: satelliteSpawnTrigged");
                satelliteSpawnTrigged.Raise();
                return;
            case BuildingComponents.BuildingType.LaunchPad:
                    if(hasBuiltLaunchPad == false){
                        SpawnNewEntity(LaunchPadPrefab);
                        hasBuiltLaunchPad = true;
                    }
                return;
        }
    }
    public void SpawnNewEntity(GameObject prefab)
    {
        Vector3 screenPos = new Vector3(375, 285, 10f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }

    public void TeleportRobotBuddy(GameObject robotBuddy){
        Vector3 screenPos = new Vector3(475, 285, 0f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        robotBuddy.transform.position = worldPos;
    }

}
