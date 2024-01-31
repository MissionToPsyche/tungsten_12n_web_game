using System.Collections;
using System.Collections.Generic;
using myEnums;
using UnityEngine;
using UnityEngine.UIElements;

namespace packet{
    /// <summary>
    /// Instantiate by using the object calling this event, the amount to change the Inventory by, the resource to change and true for addition, false for subtraction
    /// </summary>
    public class CheckInventoryPacket{
        public GameObject objectThatSent;
        public myEnums.BuildingType building;
        public ObjectsCost objCost;
        public CheckInventoryPacket(GameObject obj, myEnums.BuildingType build, ObjectsCost cost){
            objectThatSent = obj;
            building = build;
            objCost = cost;
        }
    }

    public class MiningPacket{
        public GameObject objectThatSent;
        public int amountToChange;
        public myEnums.ResourceType resourceToChange;
        public bool Add;
        public MiningPacket(GameObject obj, int amt, myEnums.ResourceType resource, bool boolean){
            objectThatSent = obj;
            amountToChange = amt;
            resourceToChange = resource;
            Add = boolean;
        }
    }

}