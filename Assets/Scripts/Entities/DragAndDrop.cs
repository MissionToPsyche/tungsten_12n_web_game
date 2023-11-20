using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler{
    
    SpriteRenderer spriteRenderer;
    readonly int ResourceLayer = 1 << 8;
    private Rigidbody2D objectBody2D;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded = true;
    private bool isPlaced = false;
    private GravityBody2D gravityBody;
    private bool dragging = false;
    private List<GravityArea2D> gravityAreas;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectBody2D = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isPlaced){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            Vector2 rayDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.down;
            //The length needs to be half of the size of the asteroid we are currently on
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.y / 2f), rayDirection, 20f, ResourceLayer);

            float targetAngle = Mathf.Atan2(gravityBody.GravityDirection.y, gravityBody.GravityDirection.x) * Mathf.Rad2Deg + 90;
            float smoothedAngle = Mathf.LerpAngle(objectBody2D.rotation, targetAngle, 180f * Time.fixedDeltaTime);

            objectBody2D.rotation = smoothedAngle;
            if(isValidPos(hit)){
                spriteRenderer.color = Color.green;
            }else{
                spriteRenderer.color = Color.red;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isPlaced)
            spriteRenderer.color = Color.red;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(spriteRenderer.color == Color.green){
            isPlaced = true;
            //Stops all possible movement
            objectBody2D.isKinematic = true;
            objectBody2D.bodyType = RigidbodyType2D.Static;
            objectBody2D.simulated = false;
            boxCollider2D.enabled = false;
        }
    }

    private bool isValidPos(RaycastHit2D hit){
        if (hit.collider != null && isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
