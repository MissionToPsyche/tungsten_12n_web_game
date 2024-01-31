using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New CheckInventory Event", menuName = "Game Events/CheckInventory Event")]

public class CheckInventoryEvent : BaseGameEvent<packet.CheckInventoryPacket>
{

}
