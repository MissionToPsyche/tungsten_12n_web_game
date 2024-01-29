using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MineEventPacket{
    public GameObject extractorThatMined;
    public int amountMined;
    public GameObject resourceMined;
    public MineEventPacket(GameObject extractor, int amt, GameObject resource){
        extractorThatMined = extractor;
        amountMined = amt;
        resourceMined = resource;
    }
}