using UnityEngine;
using BuildingComponents;
using System.IO;


public class LaunchPadManager : MonoBehaviour{

    private bool playerCanInteract = true;
    private LaunchPad launchpad;
    private int TechTier = 0;
    //building based vars
    [SerializeField] public InventoryCheckEvent checkInventory;
    private ObjectsCost engineCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost chasisCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost cockpitCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost externalTank = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    [SerializeField] private GameObject enginesPrefab;
    [SerializeField] private GameObject chasisPrefab;
    [SerializeField] private GameObject cockpitPrefab;
    [SerializeField] private GameObject externalTankPrefab;
    public void Awake(){
        launchpad = new();
    }
    public void OnPlayerInteract(){
        if(playerCanInteract){
            TryBuildNextRocketPart();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turn on current price
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {

            playerCanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        //Turn off current price
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            playerCanInteract = false;
        }
    }

    public void OnTechUpEvent(packet.TechUpPacket packet){
        if(packet.building == BuildingType.LaunchPad){
            TechTier = packet.TechToLevel;
        }
    }

    public void TryBuildNextRocketPart(){
        
        if(launchpad.isEngineBuilt()){
            if(launchpad.isChasisBuilt()){
                if(launchpad.isCockpitBuilt()){
                    if(launchpad.isExternalTankBuilt()){
                        TryBuildExternalTank();
                    }
                }else{
                    TryBuildCockpit();
                }
            }else{
                TryBuildChasis();
            }
        }else{
            TryBuildEngine();
        }
    }

    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        switch(type){
            case(BuildingComponents.BuildingType.Engines):
                SpawnEngines();
            break;
            case(BuildingComponents.BuildingType.Chasis):
                SpawnChasis();
            break;
            case(BuildingComponents.BuildingType.Cockpit):
                SpawnCockpit();
            break;
            case(BuildingComponents.BuildingType.CompWire):
                SpawnExternalTank();
            break;
        }
    }

    private void SpawnEngines(){
        Vector3 SpawnPos = transform.position;
        SpawnPos.x -= 3;
        SpawnPos.y -= 8;
        GameObject engines = Instantiate(enginesPrefab, SpawnPos, transform.rotation);
        engines.transform.parent = transform;
        launchpad.SetEngineBuilt();
    }
    private void SpawnChasis(){
        Vector3 SpawnPos = transform.position;
        SpawnPos.x -= 3;
        SpawnPos.y -= 4;
        GameObject chasis = Instantiate(chasisPrefab, SpawnPos, transform.rotation);
        chasis.transform.parent = transform;
        launchpad.SetChasisBuilt();
    }
    private void SpawnCockpit(){
        Vector3 SpawnPos = transform.position;
        SpawnPos.x -= 3;
        SpawnPos.y -= 2;
        GameObject cockpit = Instantiate(cockpitPrefab, SpawnPos, transform.rotation);
        cockpit.transform.parent = transform;
        launchpad.SetCockpitBuilt();
    }
    private void SpawnExternalTank(){
        Vector3 SpawnPos = transform.position;
        SpawnPos.x -= 3;
        SpawnPos.y += 3;
        GameObject et = Instantiate(externalTankPrefab, SpawnPos, transform.rotation);
        et.transform.parent = transform;
        launchpad.SetExternalTankBuilt();
    }
    private void TryBuildEngine(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Engines, engineCosts
                ));
    }
    private void TryBuildChasis(){
        //if has techtier 2, else no checkInventory
        Debug.Log("in here " +  TechTier);
        if(TechTier == 2){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Chasis, chasisCosts
                ));
        }
    }
    private void TryBuildCockpit(){
        if(TechTier == 3){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Cockpit, cockpitCosts
                ));
        }
    }
    private void TryBuildExternalTank(){
        if(TechTier == 4){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.CompWire, externalTank
                ));
        }
    }
}