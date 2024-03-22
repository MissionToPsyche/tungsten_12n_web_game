using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New TechUp Event", menuName = "Game Events/Custom/Build/TechUp Event")]

public class TechUpEvent : BaseGameEvent<packet.TechUpPacket>
{

}