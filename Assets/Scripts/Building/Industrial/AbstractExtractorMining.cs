using System.Collections;
using UnityEngine;
using BuildingComponents;
//This class holds the functionality of showing the the text above the extractor, 
//sending the mineEvent out and reducing the available amount of resources in the resource
public class AbstractExtractorMining : MonoBehaviour
{
    //Mini Game Event
    public VoidEvent OnMiniGameEvent;

    //Mining
    public MiningEvent OnMineEvent;
    protected float mineInterval;
    protected int amountToMine;
    protected float timer = 0;
    protected const float displayDuration = 1.5f;
    protected bool isShowingText = false;
    protected bool isBroken = false;
    protected int timesMinedSinceBroken = 0;
    protected float baseBreakChance;
    protected bool playerCanInteract = false;

    protected bool playerInteracted = false;
    //Mining UI
    public GameObject textPrefabPlusOne;
    public GameObject textPrefabExclamation;
    protected bool isLerping = false;
    private GameObject currentExtractText;
    //GameObjects
    protected GravityBody2D gravityBody;
    protected ResourceType resourceToMine;
    [SerializeField] protected bool isPlaced = false;

    protected void MineIfPlaced(){
        if(isPlaced && !isBroken){
            Mine();
        }
    }
    private void Mine()
    {
        timer += Time.deltaTime;

        if (timer >= mineInterval)
        {
            OnMineEvent.Raise(new packet.MiningPacket(this.gameObject, amountToMine, resourceToMine, true));
            timesMinedSinceBroken += 1;
            timer = 0f;
            if (RollForModuleBreak()){
                isBroken = true;
                ShowBrokeText();
                timesMinedSinceBroken = 0;
            }else{
                ShowGainText();
            }
        }

        // Check if the text is currently displayed and fade it away after display duration
        if (isShowingText)
        {
            if (timer >= displayDuration)
            {
                ResetText(currentExtractText);
            }
        }
    }
    private void ResetText(GameObject currText){
        Destroy(currText);
        timer = 0f;
        isLerping = false;
        isShowingText = false;
        currentExtractText = null;
    }
    GameObject SpawnGainText(Vector3 pos)
    {
        isShowingText = true;
        return Instantiate(textPrefabPlusOne, pos, transform.rotation);
    }
    private void ShowGainText(){
        Vector3 initPos = GetPositionTextAboveExtractor();
        currentExtractText = SpawnGainText(initPos);
        currentExtractText.GetComponent<TMPro.TextMeshPro>().SetText("+" + amountToMine.ToString());
        Vector3 newPos = new Vector3(-gravityBody.GravityDirection.x, -gravityBody.GravityDirection.y, 0f)/2;
        StartCoroutine(LerpTextPosition(currentExtractText.transform, initPos + newPos, displayDuration));
    }
    GameObject SpawnBrokeText(Vector3 pos){
        return Instantiate(textPrefabExclamation, pos, transform.rotation);
    }
    private void ShowBrokeText(){
        if(currentExtractText != null){
            ResetText(currentExtractText);
        }
        Vector3 initPos = GetPositionTextAboveExtractor();
        currentExtractText = SpawnBrokeText(initPos);
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
    private bool RollForModuleBreak(){
        float breakChance = baseBreakChance * timesMinedSinceBroken;
        //float breakChance = 0.5f;
        //^Use this for debugging or testing the playerInteract feature
        return Random.value < breakChance;
    }

    //Events
    //------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            playerCanInteract = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
        {
            playerCanInteract = false;
        }
    }
    public void OnPlayerInteract(){
        if(playerCanInteract && isBroken){
            //IMPLEMENT MINI GAME LOGIC HERE
            playerInteracted = true;
            OnMiniGameEvent.Raise();

            //isBroken = false;
            //ResetText(currentExtractText);
        }
    }

    public void fix(){
        if(playerInteracted){
            isBroken = false;
            ResetText(currentExtractText);
            playerInteracted = false;
        }
    }
}