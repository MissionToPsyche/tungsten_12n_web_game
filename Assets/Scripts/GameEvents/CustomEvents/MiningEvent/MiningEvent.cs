using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Mining Event", menuName = "Game Events/Mining Event")]

public class MiningEvent : BaseGameEvent<packet.MiningPacket>
{

}
