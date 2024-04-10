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


    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                this.gameObject.GetComponent<CaveSceneManager>().loadCaveScene(gameObject.name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.instance.GetPlayerObject())
        {
            isInRange = true;
            reminderText.text = "Enter Cave\nRemember to Avoid the Black Pits!";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.instance.GetPlayerObject())
        {
            isInRange = false;
            reminderText.text = "";
        }
    }
}
