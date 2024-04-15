using System.Collections;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private TextMeshProUGUI reminderText;  

    [Header("ReadOnly")] 
    private bool isInRange;
    private int itemValue; 
    private string collectableItem;

    // Not for display
    public static Collectable Instance {get; private set;}
    private KeyCode interactKey = KeyCode.E;
    private bool playerInteracted;
    public VoidEvent OnCaveMiniGameEvent;

    // -------------------------------------------------------------------
    // Class
    // Start is called before the first frame update
    // void Start()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else 
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    void Start()
    {
        collectableItem = this.gameObject.tag;
        
        switch (collectableItem)
        {
            case "Rock":
                switch(this.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                {
                    case "objects-bg_0":
                        itemValue = 5;
                        break; 
                    case "objects-bg_5":
                        itemValue = 5;
                        break; 
                    case "objects-bg_1":
                        itemValue = 4;
                        break; 
                    case "objects-bg_6":
                        itemValue = 4;
                        break; 
                    case "objects-bg_2":
                        itemValue = 3;
                        break; 
                    case "objects-bg_7":
                        itemValue = 3;
                        break; 
                    case "objects-bg_3":
                        itemValue = 2;
                        break; 
                    case "objects-bg_":
                        itemValue = 2;
                        break; 
                    case "objects-bg_4":
                        itemValue = 1;
                        break; 
                    case "objects-bg_9":
                        itemValue = 1;
                        break; 
                    default: 
                        itemValue = 1;
                        break;
                }
                break; 
            case "GeologicalPhenomena":
                itemValue = 1; 
                break;
            default:
                itemValue = 1;
                break;
        }
    }

    // Update is called once per frame
    public void OnPlayerInteract()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            playerInteracted = true;
            OnCaveMiniGameEvent.Raise();
        }
    }

    // update text 
    IEnumerator reminderTextCoroutine()
    {
        switch (collectableItem)
        {
            case "Rock":
                reminderText.text = "+" + itemValue.ToString() + " rock material";
                break;
            
            case "GeologicalPhenomena":
                reminderText.text = "+" + itemValue.ToString() + " skill point";
                break;
        }

        // update the UI
        OnPlayerCollect(); 
        
        yield return new WaitForSeconds(1);
        
        reminderText.text = "";
        this.gameObject.SetActive(false);
    }

    // delete the object once it comes into contact with the player
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("RobotBuddy"))
        {
            isInRange = true; 
            reminderText.text = "Collect Material";
        }
    }

    // reset text when player is out of range 
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("RobotBuddy"))
        {
            isInRange = false; 
            reminderText.text = "";
        }
    }

    // update the UI
    void OnPlayerCollect()
    {
        
    }

    public void OnCaveMiniGameWin(){
       if(playerInteracted){
         StartCoroutine(reminderTextCoroutine());
       } 
        
    }
}
