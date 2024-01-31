using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public interface IGameEventListener<T>
{
    void OnEventRaised(T item);
}
