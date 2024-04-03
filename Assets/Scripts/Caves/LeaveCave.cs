using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LeaveCave : MonoBehaviour
{
    private bool isInRange; 
    private KeyCode interactKey = KeyCode.E; 
    private GameManager gameManager; 
    private TextMeshProUGUI reminderText;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        reminderText = GameObject.FindWithTag("ReminderText").GetComponent<TextMeshProUGUI>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange) 
        {
            if (Input.GetKeyDown(interactKey)) 
            {
                gameManager.GetComponent<CaveSceneManager>().loadAsteroidScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            isInRange = true; 
            reminderText.text = "Press E to leave the cave";
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
