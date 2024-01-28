using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiningEvents/MineResourceEvent")]
public class MineResourceEvent : ScriptableObject
{
    public List<MineResourceListener> listeners = new List<MineResourceListener>();


    public void NotifyInventoryUI(Component sender, object data){
        for (int i = 0; i < listeners.Count; i++){
            listeners[i].OnEventRaised(sender, data);
        }
    }
    //Manage Listeners
    public void RegisterListener(MineResourceListener listener){
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }
    public void UnregisterListener(MineResourceListener listener){
        if(!listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
