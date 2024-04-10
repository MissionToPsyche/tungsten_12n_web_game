using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Mining Event", menuName = "Game Events/Custom/Mining/Mining Event")]

public class MiningEvent : BaseGameEvent<packet.MiningPacket>
{

}