using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropExtractor : DragAndDropSuper{
    
    private GravityBody2D gravityBody;
    private List<GravityArea2D> gravityAreas;
    public new delegate void placementEvent();
    public new static event placementEvent OnPlacementEvent;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectBody2D = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //boxCollider2D.enabled = true;
        layerToUse = SetLayerToUse();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(!isPlaced){
            //Move according to mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.y / 2f);
            Vector2 direction = gravityBody.GravityDirection;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 20f, layerToUse);
            
            if(IsValidPos(hit, rayOrigin, direction)){
                spriteRenderer.color = Color.green;
            }else{
                spriteRenderer.color = Color.red;
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(!isPlaced)
            spriteRenderer.color = Color.red;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(spriteRenderer.color == Color.green){
            isPlaced = true;
            //Lets Object know that it is placed in its final location, may start working
            OnPlacementEvent?.Invoke();
            //Stops all possible movement
            objectBody2D.isKinematic = true;
            objectBody2D.bodyType = RigidbodyType2D.Static;
            objectBody2D.simulated = false;
            boxCollider2D.enabled = false;
        }
    }

    protected bool IsValidPos(RaycastHit2D hit, Vector2 origin, Vector2 dir)
    {
        float distToGround = getDistanceToGround(origin, dir);
        if (hit.collider != null && distToGround < .2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float getDistanceToGround(Vector2 origin, Vector2 dir){
        float maxGravityDistance = 10f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxGravityDistance, 1 << 7);

        Debug.DrawRay(transform.position, dir * maxGravityDistance, Color.blue);
        if (hit.collider != null){
            return hit.distance;
        }else{
            return maxGravityDistance;
        }
    }
    protected override int SetLayerToUse()
    {
        //Resource Layer
        return 1 << 8;
    }
}
