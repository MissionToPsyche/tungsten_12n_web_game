using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New UpdateButtonCostTextEvent Event", menuName = "Game Events/Custom/UI/ButtonCostText Event")]

public class UpdateButtonCostTextEvent : BaseGameEvent<packet.UpdateButtonCostTextPacket>
{

}