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
    private ObjectsCost compWireCosts = new ObjectsCost(1,0,0,0,0,0,0,0,0);
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
                    if(launchpad.isCompWireBuilt()){
                        TryBuildCompWire();
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

    private void TryBuildEngine(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Engines, engineCosts
                ));
    }
    private void TryBuildChasis(){
        //if has techtier 2, else no checkInventory
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
    private void TryBuildCompWire(){
        if(TechTier == 4){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.CompWire, compWireCosts
                ));
        }
    }
}