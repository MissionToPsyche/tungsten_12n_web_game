using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using BuildingComponents;
//This class holds the functionality of showing the the text above the extractor, sending the mineEvent out and reducing the available amount of resources in the resource
public class ExtractorMining : MonoBehaviour
{
    public float mineSpeed = 4f;
    private float timer = 3f;
    private const float displayDuration = 1.5f; // Duration to display the +1 text
    private bool isShowingText = false;
    private GravityBody2D gravityBody;
    public GameObject textPrefabPlusOne;
    public GameObject textPrefabExclamation;
    public GameObject currentExtractText;
    private ResourceType resourceToMine;
    public MiningEvent OnMineEvent;
    [SerializeField] private bool isPlaced = false;
    private bool isLerping = false;


    void Start(){
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
    }

    void Update(){
        mineResource();
    }

    private void mineResource(){
        if(isPlaced){
            Mine();
        }
    }
    private void Mine()
    {
        timer += Time.deltaTime;

        // Check if it's time to display the +1 text
        if (timer >= mineSpeed)
        {
            OnMineEvent.Raise(new packet.MiningPacket(this.gameObject, 1, resourceToMine, true));
            timer = 0f;
            showText();
        }

        // Check if the text is currently displayed and fade it away after display duration
        if (isShowingText)
        {
            if (timer >= displayDuration)
            {
                Destroy(currentExtractText);
                timer = 0f;
                isLerping = false;
                isShowingText = false;
                currentExtractText = null;
            }
        }
    }
    private void showText(){
        Vector3 initPos = GetPositionTextAboveExtractor();
        currentExtractText = SpawnExtractText(initPos);
        Vector3 newPos = new Vector3(-gravityBody.GravityDirection.x, -gravityBody.GravityDirection.y, 0f)/2;
        StartCoroutine(LerpTextPosition(currentExtractText.transform, initPos + newPos, displayDuration));
    }
    IEnumerator LerpTextPosition(Transform textTransform, Vector3 targetPos, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPos = textTransform.position;

        while (elapsedTime < duration && textTransform != null)
        {
            textTransform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (textTransform){
            textTransform.position = targetPos;
        }
    }
    GameObject SpawnExtractText(Vector3 pos)
    {
        isShowingText = true;
        return Instantiate(textPrefabPlusOne, pos, transform.rotation);
    }

    Vector3 GetPositionTextAboveExtractor()
    {
        //Position above the extractor relative to extractor
        Vector3 newPos = new Vector3(-gravityBody.GravityDirection.x, -gravityBody.GravityDirection.y, 0f)*2.3f;
        return transform.position + newPos;
    }
    public void LinkToResource(GameObject resourceObject){
        isPlaced = true;
        resourceToMine = ConvertStrToResourceType(resourceObject.name);
    }

    private void OnDestroy(){
        isShowingText = false;
        DragAndDropExtractor.OnPlacementEvent -= LinkToResource;
    }

    private ResourceType ConvertStrToResourceType(string str){
        int underScoreIndex = str.IndexOf('_');

        if(underScoreIndex != -1){
            switch(str.Substring(0, underScoreIndex)){
                case "Iron":
                    return ResourceType.Iron;
                case "Nickel":
                    return ResourceType.Nickel;
                case "Cobalt":
                    return ResourceType.Cobalt;
                case "Platinum":
                    return ResourceType.Platinum;
                case "Gold":
                    return ResourceType.Gold;
                case "Technitium":
                    return ResourceType.Technitium;
                case "Tungsten":
                    return ResourceType.Tungsten;
                case "Iridium":
                    return ResourceType.Iridium;
                default:
                    Debug.LogError("ExtractorMining.cs --: ConvertStrToResourceType() :-- Switch Statement missed all cases, returning Iron!");
                    return ResourceType.Iron;
            }
        }else{
            Debug.LogError("ExtractorMining.cs --: ConvertStrToResourceType() :-- Recieved MineEvent Packet does not have correct resource naming, returning Iron!");
            return ResourceType.Iron;
        }
    }
}