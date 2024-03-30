using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LeaveCave : MonoBehaviour
{
    public bool isInRange; 
    public KeyCode interactKey; 
    public UnityEvent interactAction; 
    public TextMeshProUGUI reminderText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange) 
        {
            if (Input.GetKeyDown(interactKey)) 
            {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            isInRange = true; 
            reminderText.text = "Press E to leave cave.";
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
