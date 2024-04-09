using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CollectableRock : MonoBehaviour
{
    private bool isInRange;
    private KeyCode interactKey = KeyCode.E;
    private TextMeshProUGUI reminderText;
    private int rockValue; 
    // Start is called before the first frame update
    void Start()
    {
        reminderText = GameObject.FindWithTag("ReminderText").GetComponent<TextMeshProUGUI>();
        
        switch(this.gameObject.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "objects-bg_0":
                rockValue = 5;
                break; 
            case "objects-bg_5":
                rockValue = 5;
                break; 
            case "objects-bg_1":
                rockValue = 4;
                break; 
            case "objects-bg_6":
                rockValue = 4;
                break; 
            case "objects-bg_2":
                rockValue = 3;
                break; 
            case "objects-bg_7":
                rockValue = 3;
                break; 
            case "objects-bg_3":
                rockValue = 2;
                break; 
            case "objects-bg_":
                rockValue = 2;
                break; 
            case "objects-bg_4":
                rockValue = 1;
                break; 
            case "objects-bg_9":
                rockValue = 1;
                break; 
            default: 
                rockValue = 1;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(reminderTextCoroutine());
        }
    }

    // update text 
    IEnumerator reminderTextCoroutine()
    {
        reminderText.text = "+" + rockValue.ToString();
        
        yield return new WaitForSeconds(1);
        
        reminderText.text = "";
        Destroy(this.gameObject);
    }

    // delete the object once it comes into contact with the player
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Player"))
        {
            isInRange = true; 
            reminderText.text = "Collect Material";
        }
    }
}
