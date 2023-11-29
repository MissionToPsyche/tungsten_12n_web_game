using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class ExtractorMining : MonoBehaviour
{
    public float mineSpeed = 4f;
    private float timer = 3f;
    private const float displayDuration = 1.5f; // Duration to display the +1 text
    private bool isShowingText = false;

    public GameObject textPrefabPlusOne;
    public GameObject textPrefabExclamation;
    public GameObject currentExtractText;
    [SerializeField] private bool isPlaced = false;
    private bool isLerping = false;

    //PURELY FOR DEMENSTRATION PURPOSES
    private int shownTwice = 0;
    void Start(){
        DragAndDrop.OnPlacementEvent += SetObjectPlaced;
    }

    void Update(){
        mineResource();
    }

    private void mineResource(){
        if(isPlaced){
            ShowAndFadeText();
        }
    }
    void ShowAndFadeText()
    {
        timer += Time.deltaTime;

        // Check if it's time to display the +1 text
        if (timer >= mineSpeed)
        {
            if(shownTwice == 2){
                shownTwice += 1;
                GameObject newObj = Instantiate(textPrefabExclamation, GetPositionTextAboveExtractor() + new Vector3(0f, 1f, 0f), transform.rotation);
            }else if(shownTwice < 2){
                timer = 0f;
                currentExtractText = SpawnExtractText();
            }
        }

        // Check if the text is currently displayed and fade it away after display duration
        if (isShowingText)
        {
            if (timer >= displayDuration && shownTwice < 2)
            {
                timer = 0f;
                Destroy(currentExtractText);
                shownTwice += 1;
                isLerping = false;
                isShowingText = false;
                currentExtractText = null;
            }else{
                if(!isLerping){
                    isLerping = true;
                    Vector3 targetPos = currentExtractText.transform.position + new Vector3(0.0f, 1f, 0.0f);
                    currentExtractText.transform.position = Vector3.Lerp(currentExtractText.transform.position, targetPos, displayDuration);
                }
            }
        }


    }

    GameObject SpawnExtractText()
    {
        isShowingText = true;
        // GameObject newObj = Instantiate(textPrefab, GetPositionTextAboveExtractor(), Quaternion.identity);
        return Instantiate(textPrefabPlusOne, GetPositionTextAboveExtractor(), transform.rotation);
    }
    void FadeOutExtractText()
    {
        if (currentExtractText != null)
        {
            Destroy(currentExtractText.gameObject);
            isShowingText = false;
        }
    }
    Vector3 GetPositionTextAboveExtractor()
    {
        return new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
    }
    public void SetObjectPlaced(){
        isPlaced = true;
    }

    private void OnDestroy(){
        DragAndDrop.OnPlacementEvent -= SetObjectPlaced;
    }
}
