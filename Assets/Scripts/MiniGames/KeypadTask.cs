using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadTask : MonoBehaviour
{
    public TextMeshProUGUI cardCode;

    public TextMeshProUGUI inputCode;

    public int codeLength = 5;

    public float codeResetTimeInSeconds = 0.5f;

    private bool isResetting = false; 

    public BoolEvent winCondition;

    private void OnEnable(){
        string code = string.Empty;

        for (int i = 0; i < codeLength; i++){
            code += Random.Range(1,10);
        }

        cardCode.text = code;
        inputCode.text = string.Empty;
    }

    public void ButtonClick(int number){
        if(isResetting) { return; }

        inputCode.text += number;

        if(inputCode.text == cardCode.text){
            inputCode.text = "Correct";
            winCondition.Raise(true);
        }
        else if(inputCode.text.Length >= codeLength){
            inputCode.text = "failed";
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode(){
        isResetting = true;

        yield return new WaitForSeconds(codeResetTimeInSeconds);

        inputCode.text = string.Empty;

        isResetting = false;
    }
   
}
