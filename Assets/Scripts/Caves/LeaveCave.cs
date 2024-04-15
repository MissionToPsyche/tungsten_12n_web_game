using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LeaveCave : MonoBehaviour
{
    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private TextMeshProUGUI reminderText;  

    [Header("ReadOnly")]


    private bool isInRange;
    private KeyCode interactKey = KeyCode.E;
    private GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            //Debug.Log(gameObject.scene.name);
            string CaveScene = gameObject.scene.name;
            CaveManager.Instance.LeaveCaveScene(CaveScene);
            PlayerManager.Instance.SetScenePosition("AsteroidScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.GetPlayerObject())
        {
            isInRange = true;
            reminderText.text = "Leave Cave";
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
