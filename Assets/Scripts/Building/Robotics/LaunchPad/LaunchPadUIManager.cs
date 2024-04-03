using UnityEngine;
using BuildingComponents;
using System.IO;

public class LaunchPadUIManager : MonoBehaviour{

    [SerializeField] public GameObject BuildRocketModuleUI;
    [SerializeField] VoidEvent buttonPressEvent;

    public void OnTriggerRocketModuleUIOverlay(bool setTo){
        BuildRocketModuleUI.SetActive(setTo);
    }
    public void RocketModuleButtonPressed(){
        buttonPressEvent.Raise();
    }
}