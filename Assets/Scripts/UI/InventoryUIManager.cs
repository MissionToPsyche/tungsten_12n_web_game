using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryOverlay;
    // Start is called before the first frame update

    [Header("Events")]
    public MineResourceEvent OnInventoryChange;

    void Awake(){
        
    }
    public void UpdateInventory(Component sender, object data)
    {
        // Implement the logic to update your UI based on minedAmount
        Debug.Log("rahh");
        Debug.Log("Mining event received! Sender: " + sender + ", Data: " + data);
    }


}
