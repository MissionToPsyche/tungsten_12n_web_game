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
        reminderText = GameObject.FindWithTag("ReminderText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            CaveManager.instance.LoadAsteroidScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.instance.GetPlayerObject())
        {
            isInRange = true;
            reminderText.text = "Leave Cave";
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
