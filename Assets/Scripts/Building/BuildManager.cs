using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] public GameObject ExtractorPrefab;
    [SerializeField] public GameObject CommercialExtractorPrefab;
    [SerializeField] public GameObject IndustrialExtractorPrefab;

    [SerializeField] public GameObject LaunchPadPrefab;
    private UIBuildManager buildUI;
    void Awake(){
        buildUI = GetComponent<UIBuildManager>();
    }
    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        //Turns off Build overlay after an obj has been bought
        buildUI.OnPlayerBuildOverlay();
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

                return;
            case BuildingComponents.BuildingType.Satellite:

                return;
            case BuildingComponents.BuildingType.LaunchPad:
                    SpawnNewEntity(LaunchPadPrefab);
                return;
                //Will need 4 more for Rocket parts
        }
    }
    public void SpawnNewEntity(GameObject prefab)
    {
        Vector3 screenPos = new Vector3(375, 285, 10f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }

    public void SpawnRocketPart(){
        //makes the rocket part show up next to the launchpad
    }
}
