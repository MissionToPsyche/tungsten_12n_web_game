using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {}
public class MineResourceListener : MonoBehaviour
{
    public CustomGameEvent response;
    
    public MineResourceEvent gameEvent;

    public void OnEventRaised(Component sender, object data){
        response.Invoke(sender, data);
    }
    private void OnEnable(){
        gameEvent?.RegisterListener(this);
    }

    private void OnDisable(){
        gameEvent?.UnregisterListener(this);
    }

    
}
