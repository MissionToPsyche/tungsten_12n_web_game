using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InventoryTypes;

public class Inventory
{
    Dictionary<ResourceType, int> inv = new Dictionary<ResourceType, int>();

    public Inventory(int IronAmt, int NickelAmt, int SilverAmt, 
    int PlatinumAmt, int GoldAmt, int TechnitiumAmt, int TungstenAmt, int IridiumAmt){
        inv.Add(ResourceType.Iron, IronAmt);
        inv.Add(ResourceType.Nickel, NickelAmt);
        inv.Add(ResourceType.Silver, SilverAmt);
        inv.Add(ResourceType.Platinum, PlatinumAmt);
        inv.Add(ResourceType.Gold, GoldAmt);
        inv.Add(ResourceType.Technitium, TechnitiumAmt);
        inv.Add(ResourceType.Tungsten, TungstenAmt);
        inv.Add(ResourceType.Iridium, IridiumAmt);
    }
    public Dictionary<ResourceType, int> GetInvDictionary(){
        return inv;
    }
    public bool CheckCost(ObjectsCost resourcesToCheck){
        foreach(var resource in resourcesToCheck.getCostDictionary()){
            //amt had < amt needed, if so cant buy
            if(inv[resource.Key] < resource.Value){
                return false;
            }
        }
        //has all resources required
        return true;
    }
    public void PayForObjectWithObjCost(ObjectsCost costs){
        foreach(var entry in costs.getCostDictionary()){
            SubResource(entry.Key, entry.Value);
        }
    }
    public void AddResource(ResourceType resourceType, int amt){
        switch(resourceType){
            case(ResourceType.Iron):
                inv[ResourceType.Iron] += amt;
                break;
            case(ResourceType.Nickel):
                inv[ResourceType.Nickel] += amt;
                break;
            case(ResourceType.Silver):
                inv[ResourceType.Silver] += amt;
                break;
            case(ResourceType.Platinum):
                inv[ResourceType.Platinum] += amt;
                break;
            case(ResourceType.Gold):
                inv[ResourceType.Gold] += amt;
                break;
            case(ResourceType.Technitium):
                inv[ResourceType.Technitium] += amt;
                break;
            case(ResourceType.Tungsten):
                inv[ResourceType.Tungsten] += amt;
                break;
            case(ResourceType.Iridium):
                inv[ResourceType.Iridium] += amt;
                break;
        }
    }

    public void SubResource(ResourceType resourceType, int amt){
        switch(resourceType){
            case(ResourceType.Iron):
                inv[ResourceType.Iron] -= amt;
                break;
            case(ResourceType.Nickel):
                inv[ResourceType.Nickel] -= amt;
                break;
            case(ResourceType.Silver):
                inv[ResourceType.Silver] -= amt;
                break;
            case(ResourceType.Platinum):
                inv[ResourceType.Platinum] -= amt;
                break;
            case(ResourceType.Gold):
                inv[ResourceType.Gold] -= amt;
                break;
            case(ResourceType.Technitium):
                inv[ResourceType.Technitium] -= amt;
                break;
            case(ResourceType.Tungsten):
                inv[ResourceType.Tungsten] -= amt;
                break;
            case(ResourceType.Iridium):
                inv[ResourceType.Iridium] -= amt;
                break;
        }
    }

}