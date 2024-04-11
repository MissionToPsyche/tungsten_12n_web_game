using UnityEngine;
using System.Collections;

public class Cybernetics : BaseBuilding
{
    private MonoBehaviour monoBehaviour;
    private int numCharges;
    private int currentMaxCharge;
    private int initMaxCharge = 1;
    private int tier1MaxCharge = 3;
    private int tier3MaxCharge = 6;
    private bool isBuilt = false;
    private float initWaitAmount = 420;
    private float currentWaitAmount;
    private float tier1WaitAmount = 300;
    private float tier2WaitAmount = 210;
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.Cybernetics;
    public Cybernetics(MonoBehaviour monoBehaviour){
        currentTier = 0;
        this.monoBehaviour = monoBehaviour;
        buildingData = LoadBuildingData();
        BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("LaunchPad");
        thisCosts = InitObjCost(thisObject);
    }
    
    
    public void StartCyberneticCoroutine(){
        monoBehaviour.StartCoroutine(ChargeCoroutine());
    }
    private IEnumerator ChargeCoroutine(){
        if(numCharges < currentMaxCharge){
            numCharges += 1;
        }
        yield return new WaitForSeconds(currentWaitAmount);
    }

    public bool HasCharge(){
        return numCharges > 0;
    }
    public void UseCharge(){
        if(numCharges == 0){
            Debug.LogWarning("[Cybernetics.cs] -: UseCharge() used a charge when there was none available!");
            return;
        }
        numCharges -= 1;
    }
    public void UpdateTechTier(){
        currentTier = InventoryManager.Instance.GetTechTier(BuildingComponents.BuildingType.Cybernetics);
        //Debug.Log(InventoryManager.Instance.GetTechTier(BuildingComponents.BuildingType.Cybernetics));
        if(currentTier == 0){
            currentWaitAmount = initWaitAmount;
            currentMaxCharge = initMaxCharge;
        }else if(currentTier == 1 || currentTier == 2){
            currentWaitAmount = tier1WaitAmount;
            currentMaxCharge = tier1MaxCharge;
        }else if(currentTier == 3){
            currentWaitAmount = tier2WaitAmount;
            currentMaxCharge = tier3MaxCharge;
        }
    }
    public bool IsBuilt(){
        return isBuilt;
    }
    public void SetBuilt(){
        isBuilt = true;
        numCharges = 1;
    }
}