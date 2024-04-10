using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New CheckInventory Event", menuName = "Game Events/Custom/Inventory/CheckInventory Event")]

public class InventoryCheckEvent : BaseGameEvent<packet.CheckInventoryPacket>
{

}