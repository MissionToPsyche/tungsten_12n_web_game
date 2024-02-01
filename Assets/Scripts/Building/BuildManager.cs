using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject prefab;

    static private Extractor extractor = new();
    private BuildUIManager buildUI;
    void Awake(){
        buildUI = GetComponent<BuildUIManager>();
    }
    public void OnBuildObjEvent(InventoryTypes.BuildingType type){
        //Turns off Build overlay after an obj has been bought
        buildUI.OnPlayerBuildOverlay();
        switch(type){
            case InventoryTypes.BuildingType.Extractor:
                SpawnNewExtractor();
                return;
        }
    }
    public void SpawnNewExtractor()
    {
        Vector3 screenPos = new Vector3(375, 285, 10f); 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }
    public ObjectsCost GetExtractorCost(){
        return extractor.getCostDictionary();
    }
}
