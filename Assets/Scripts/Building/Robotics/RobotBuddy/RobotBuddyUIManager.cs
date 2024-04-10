using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RobotUIBuddyManager: MonoBehaviour
{
    [SerializeField] private GameObject alphaUIOverlay;
    [SerializeField] private GameObject betaUIOverlay;
    [SerializeField] private Image chargeFillAlphaMeter;
    [SerializeField] private Image chargeFillBetaMeter;
    private Control.State currentState = Control.State.Player;
    private readonly float fullFillAmt = 1;
    private readonly float maxChargeAmt = 600.0f;
    private float alphaFillAmt = 1;
    private float betaFillAmt = 1;
    public void Start(){
        alphaUIOverlay.SetActive(false);
        betaUIOverlay.SetActive(false);
    }

    public void OnControlStateUpdated(Control.State newState){
        currentState = newState;
        UpdateUIOnState();
    }
    public void OnAdjustRobotUI(float charge){
        float clampVal = Clamp01(charge);
        if(currentState == Control.State.RobotBuddyAlpha){
            chargeFillAlphaMeter.fillAmount = clampVal;
        }else if(currentState == Control.State.RobotBuddyBeta){
            chargeFillAlphaMeter.fillAmount = clampVal;
        }
    }

    private void UpdateUIOnState(){
        if(currentState == Control.State.RobotBuddyAlpha)
        {
            alphaUIOverlay.SetActive(true);
            betaUIOverlay.SetActive(false);
        }else if(currentState == Control.State.RobotBuddyBeta){
            betaUIOverlay.SetActive(true);
            alphaUIOverlay.SetActive(false);
        }else{
            alphaUIOverlay.SetActive(false);
            betaUIOverlay.SetActive(false);
        }
    }

    private float Clamp01(float val){
        return val / maxChargeAmt;
    }
}