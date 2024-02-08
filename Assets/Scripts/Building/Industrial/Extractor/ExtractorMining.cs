using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using BuildingComponents;
//This class holds the functionality of showing the the text above the extractor, sending the mineEvent out and reducing the available amount of resources in the resource
public class ExtractorMining : AbstractExtractorMining
{
    Extractor extractorClass;
    void Start(){
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
        extractorClass = new();
        mineInterval = extractorClass.GetMineInterval();
        amountToMine = extractorClass.GetAmountToMine();
        baseBreakChance = extractorClass.GetBaseBreakChance();
    }

    void Update(){
        MineIfPlaced();
    }
}