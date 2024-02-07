using System.Collections.Generic;
using UnityEngine;

namespace BuildingComponents
{

    public enum ResourceType
    {
        Iron,
        Nickel,
        Cobalt,
        Platinum,
        Gold,
        Technitium,
        Tungsten,
        Iridium
    }

    public enum BuildingType
    {
        Extractor,
        CommercialExtractor,
        IndustrialExtractor,
        MasterKey,
        Satellite,
        JetPack,
        CommercialCommunications,
        ExoSuit,
        RobotBuddy
    }
    
    //JSON Serialization
    //----------------------------------------------------------------------------------------
    public class BuildingObject
    {
        public string ID;
        public string Type;
        public string ItemTitle;
        public string ItemDescription;
        public int AmountToMine;
        public int IntervalMine;
        public Dictionary<string, int> Costs;
        public string Image;
    }

    [System.Serializable]
    public class BuildingData
    {
        public List<BuildingObject> BuildingObject;
    }
    
}
