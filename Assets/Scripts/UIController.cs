using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button switchContextButton;
    public Character character;

    private void Update()
    {
        // Enable or disable the button based on the character's grounded status
        switchContextButton.interactable = character.IsCharacterGrounded();
    }

    public void OnSwitchContextButtonClick()
    {
        if (character.IsCharacterGrounded())
        {
            ContextEngine.Instance.SwitchControlState();
        }
    }
}
