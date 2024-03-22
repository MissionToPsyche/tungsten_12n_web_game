using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField ]public GameObject ExtractorPrefab;
    [SerializeField ]public GameObject CommercialExtractorPrefab;
    [SerializeField ]public GameObject IndustrialExtractorPrefab;
    private UIBuildManager buildUI;
    void Awake(){
        buildUI = GetComponent<UIBuildManager>();
    }
    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        //Turns off Build overlay after an obj has been bought
        buildUI.OnPlayerBuildOverlay();
        switch(type){
            case BuildingComponents.BuildingType.Extractor:
                SpawnNewExtractor(ExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.CommercialExtractor:
                SpawnNewExtractor(CommercialExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.IndustrialExtractor:
                SpawnNewExtractor(IndustrialExtractorPrefab);
                return;
        }
    }
    public void SpawnNewExtractor(GameObject prefab)
    {
        Vector3 screenPos = new Vector3(375, 285, 10f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }
}
