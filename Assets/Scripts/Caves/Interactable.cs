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
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            string CaveScene = gameObject.tag;
            Transform CaveSpawn = CaveManager.Instance.EnterCaveScene(CaveScene);
            PlayerManager.Instance.SetLastPosition();
            PlayerManager.Instance.SetScenePosition(CaveScene, CaveSpawn);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.GetPlayerObject())
        {
            isInRange = true;
            reminderText.text = "Enter Cave\nRemember to Avoid the Black Pits!";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.GetPlayerObject())
        {
            isInRange = false;
            reminderText.text = "";
        }
    }
}
