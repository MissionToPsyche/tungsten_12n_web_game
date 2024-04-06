using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New SpawnResource Event", menuName = "Game Events/Custom/Spawning/SpawnResource Event")]

public class SpawnResourceEvent : BaseGameEvent<packet.ResourceGameObjectPacket>
{

}