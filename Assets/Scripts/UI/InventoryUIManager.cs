using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryOverlay;
    [SerializeField] private TextMeshProUGUI IronInvText;
    [SerializeField] private TextMeshProUGUI NickelInvText;
    [SerializeField] private TextMeshProUGUI SilverInvText;
    [SerializeField] private TextMeshProUGUI PlatinumInvText;
    [SerializeField] private TextMeshProUGUI GoldInvText;
    [SerializeField] private TextMeshProUGUI TechnitiumInvText;
    [SerializeField] private TextMeshProUGUI TungestenInvText;
    [SerializeField] private TextMeshProUGUI IridiumInvText;

    [Header("Events")]
    public MineResourceEvent OnInventoryChange;

    void Awake(){

    }
    public void UpdateInventory(MineEventPacket packet)
    {
        updateInventoryText(parseResourceText(packet.resourceMined.name), packet.amountMined);
    }
    
    private void updateInventoryText(string str, int amt){
        switch(str){
            case "Iron":
                IronInvText.text = (int.Parse(IronInvText.text) + amt).ToString();
                break;
            case "Nickel":
                NickelInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Silver":
                SilverInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Platinum":
                PlatinumInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Gold":
                GoldInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Technitium":
                TechnitiumInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Tungsten":
                TungestenInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
            case "Iridium":
                IridiumInvText.text = (int.Parse(NickelInvText.text) + amt).ToString();
                break;
        }
    }

    private void parsePacketForResource(string minedResource){

        switch(minedResource){

        }
    }
    private string parseResourceText(string str){
        int underScoreIndex = str.IndexOf('_');

        if(underScoreIndex != -1){
            return str.Substring(0, underScoreIndex);
        }else{
            Debug.LogError("InventoryUIManager.cs --: parseResourceText() :-- Recieved MineEvent Packet does not have correct resource naming!");
            return null;
        }
    }
}


