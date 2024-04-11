using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using BuildingComponents;

namespace packet{
    /// <summary>
    /// Instantiate by using the object calling this event, the amount to change the Inventory by, the resource to change and true for addition, false for subtraction
    /// </summary>
    public class CheckInventoryPacket{
        public GameObject objectThatSent;
        public BuildingType building;
        public ObjectsCost objCost;
        public CheckInventoryPacket(GameObject obj, BuildingType build, ObjectsCost cost){
            objectThatSent = obj;
            building = build;
            objCost = cost;
        }
    }

    public class MiningPacket{
        //Obj is the sender, used purely for debugging
        public GameObject obj;
        public int amountToChange;
        public ResourceType resourceToChange;
        public bool Add;
        public MiningPacket(GameObject obj, int amt, ResourceType resource, bool boolean){
            this.obj = obj;
            amountToChange = amt;
            resourceToChange = resource;
            Add = boolean;
        }
    }

    public class UpdateButtonCostTextPacket{
        public GameObject objectThatSent;
        public Dictionary<ResourceType, int> inventory;
        public UpdateButtonCostTextPacket(Dictionary<ResourceType, int> inventory){
            this.inventory = inventory;
        }
    }

    public class TechUpPacket{
        public int TechToLevel;
        public BuildingComponents.BuildingType building;
        public TechUpPacket(BuildingComponents.BuildingType building, int TechToLevel){
            this.building = building;
            this.TechToLevel = TechToLevel;
        }
    }
    public class ResourceGameObjectPacket{
        public GameObject GameObjectResource;
        public Resource resource;

        public ResourceGameObjectPacket(GameObject GameObjectResource, Resource resource){
            this.GameObjectResource = GameObjectResource;
            this.resource = resource;
        }
    }
}