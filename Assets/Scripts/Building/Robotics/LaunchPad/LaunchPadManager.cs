using UnityEngine;
using BuildingComponents;

[DefaultExecutionOrder(100)]
public class LaunchPadManager : MonoBehaviour{

    private bool isPlaced = false;
    private LaunchPad launchpad;
    private int TechTier = 0;
    [SerializeField] BoolEvent triggerRocketModuleUIOverlay;
    //building based vars
    [SerializeField] public InventoryCheckEvent checkInventory;
    private ObjectsCost engineCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost chasisCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost cockpitCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    private ObjectsCost externalTankCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
    [SerializeField] private GameObject enginesPrefab;
    [SerializeField] private GameObject chasisPrefab;
    [SerializeField] private GameObject cockpitPrefab;
    [SerializeField] private GameObject externalTankPrefab;
    [SerializeField] protected BuildObjEvent queryTechEvent;
    public void Awake(){
        launchpad = new();
        QueryTechLevel();
    }
    public void OnUIButtonPress(){
        TryBuildNextRocketPart();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 && isPlaced)
        {
            triggerRocketModuleUIOverlay.Raise(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        //Turn off current price
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 && isPlaced)
        {
            triggerRocketModuleUIOverlay.Raise(false);
        }
    }
    public void SetPlaced(){
        isPlaced = true;
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
                    if(!launchpad.isExternalTankBuilt()){
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
            case(BuildingComponents.BuildingType.ExternalTank):
                SpawnExternalTank();
            break;
        }
    }

    private void SpawnEngines(){
        Vector3 SpawnPos = transform.position;
        //SpawnPos.x -= 2.05f;
        //SpawnPos.y -= 6f;
        GameObject engines = Instantiate(enginesPrefab, SpawnPos, transform.rotation);
        engines.transform.parent = transform;
        launchpad.SetEngineBuilt();
    }
    private void SpawnChasis(){
        Vector3 SpawnPos = transform.position;
        //SpawnPos.x -= 3;
        //SpawnPos.y -= 4;
        GameObject chasis = Instantiate(chasisPrefab, SpawnPos, transform.rotation);
        chasis.transform.parent = transform;
        launchpad.SetChasisBuilt();
    }
    private void SpawnCockpit(){
        Vector3 SpawnPos = transform.position;
        //SpawnPos.x -= 1.5f;
        //SpawnPos.y -= 1.5f;
        GameObject cockpit = Instantiate(cockpitPrefab, SpawnPos, transform.rotation);
        cockpit.transform.parent = transform;
        launchpad.SetCockpitBuilt();
    }
    private void SpawnExternalTank(){
        Vector3 SpawnPos = transform.position;
        //SpawnPos.x -= 5f;
        //SpawnPos.y -= 2;
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
        if(TechTier >= 2){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Chasis, chasisCosts
                ));
        }
    }
    private void TryBuildCockpit(){
        if(TechTier >= 3){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Cockpit, cockpitCosts
                ));
        }
    }
    private void TryBuildExternalTank(){
        if(TechTier >= 4){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.ExternalTank, externalTankCosts
                ));
        }
    }
    protected void QueryTechLevel(){
        queryTechEvent.Raise(BuildingType.LaunchPad);
    }
    //Events
    //------------------------------------------------------------------
    public void OnQueryTechResponse(packet.TechUpPacket packet){
        if(packet.building == BuildingType.LaunchPad){
            TechTier = packet.TechToLevel;
        }
    }
}