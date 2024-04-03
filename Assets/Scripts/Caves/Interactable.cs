using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private bool isInRange; 
    private KeyCode interactKey = KeyCode.E; 
    public TextMeshProUGUI reminderText;

    private GameManager gameManager; 

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange) 
        {
            if (Input.GetKeyDown(interactKey)) 
            {
                // dynamically locate the player manager and set the last position, then invoke the scene change
                gameManager.GetComponent<PlayerManager>().setLastPosition();
                gameManager.GetComponent<CaveSceneManager>().loadCaveScene(gameObject.name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            isInRange = true; 
            reminderText.text = "Press E to enter the cave\nRemember avoid the pits";
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            isInRange = false; 
            reminderText.text = "";
        }
    }
}
