using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InventoryTypes;

public class ObjectsCost
{
    Dictionary<ResourceType, int> inv = new Dictionary<ResourceType, int>();
    public ObjectsCost(int IronAmt, int NickelAmt, int SilverAmt, 
    int PlatinumAmt, int GoldAmt, int TechnitiumAmt, int TungstenAmt, int IridiumAmt){
        if(IronAmt != 0)
            inv.Add(ResourceType.Iron, IronAmt);
        if(NickelAmt != 0)
            inv.Add(ResourceType.Nickel, NickelAmt);
        if(SilverAmt != 0)
            inv.Add(ResourceType.Silver, SilverAmt);
        if(PlatinumAmt != 0)
            inv.Add(ResourceType.Platinum, PlatinumAmt);
        if(GoldAmt != 0)
            inv.Add(ResourceType.Gold, GoldAmt);
        if(TechnitiumAmt != 0)
            inv.Add(ResourceType.Technitium, TechnitiumAmt);
        if(TungstenAmt != 0)
            inv.Add(ResourceType.Tungsten, TungstenAmt);
        if(IridiumAmt != 0)
            inv.Add(ResourceType.Iridium, IridiumAmt);
    }

    public Dictionary<ResourceType, int> getCostDictionary(){
        return inv;
    }
}