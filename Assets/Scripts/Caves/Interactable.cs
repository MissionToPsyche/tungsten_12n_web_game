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
    [SerializeField] public SoundEffectEvent soundEffectEvent;

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                packet.SoundEffectPacket sfxpacket = new packet.SoundEffectPacket(gameObject, SFX.Cave.Enter);
                soundEffectEvent.Raise(sfxpacket);
                packet.SoundEffectPacket sfxpacket2 = new packet.SoundEffectPacket(gameObject, SFX.Cave.Ambience);
                soundEffectEvent.Raise(sfxpacket2);
                CaveManager.Instance.LoadCaveScene(gameObject.name);
            }
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
