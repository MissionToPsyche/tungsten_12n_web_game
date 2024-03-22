using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BuildingComponents;
public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryOverlay;
    [SerializeField] private TextMeshProUGUI IronInvText;
    [SerializeField] private TextMeshProUGUI NickelInvText;
    [SerializeField] private TextMeshProUGUI CobaltInvText;
    [SerializeField] private TextMeshProUGUI PlatinumInvText;
    [SerializeField] private TextMeshProUGUI GoldInvText;
    [SerializeField] private TextMeshProUGUI TechnitiumInvText;
    [SerializeField] private TextMeshProUGUI TungestenInvText;
    [SerializeField] private TextMeshProUGUI IridiumInvText;
    [SerializeField] private TextMeshProUGUI TechPointsInvText;
    
    public void UpdateInventoryFromDictionary(Dictionary<ResourceType, int> dict){
        foreach(var entry in dict)
        {
            UpdateInventoryText(entry.Key, entry.Value);
        }
    }
    private void UpdateInventoryText(ResourceType type, int amt){
        switch(type){
            case ResourceType.Iron:
                IronInvText.text = amt.ToString();
                break;
            case ResourceType.Nickel:
                NickelInvText.text = amt.ToString();
                break;
            case ResourceType.Cobalt:
                CobaltInvText.text = amt.ToString();
                break;
            case ResourceType.Platinum:
                PlatinumInvText.text = amt.ToString();
                break;
            case ResourceType.Gold:
                GoldInvText.text = amt.ToString();
                break;
            case ResourceType.Technitium:
                TechnitiumInvText.text = amt.ToString();
                break;
            case ResourceType.Tungsten:
                TungestenInvText.text = amt.ToString();
                break;
            case ResourceType.Iridium:
                IridiumInvText.text = amt.ToString();
                break;
            case ResourceType.TechPoint:
                TechPointsInvText.text = amt.ToString();
                break;
        }
    }
}