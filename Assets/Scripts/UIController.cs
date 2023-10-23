// using UnityEngine;
// using UnityEngine.UI;

// public class UIController : MonoBehaviour
// {
//     public Button switchContextButton;
//     public Character character;
//     public Canvas gameCanvas;
//     public Cinemachine.CinemachineVirtualCamera characterCamera;
//     public Cinemachine.CinemachineVirtualCamera spaceshipCamera;

//     private void Update()
//     {
//         // Show or hide the button based on the character's grounded status
//         switchContextButton.gameObject.SetActive(character.IsCharacterGrounded());

//         // Set canvas camera based on current active camera
//         if (ContextEngine.Instance.currentControlState == ContextEngine.ControlState.Character)
//         {
//             gameCanvas.worldCamera = characterCamera.VirtualCameraGameObject.GetComponent<Camera>();
//         }
//         else
//         {
//             gameCanvas.worldCamera = spaceshipCamera.VirtualCameraGameObject.GetComponent<Camera>();
//         }
//     }

//     public void OnSwitchContextButtonClick()
//     {
//         if (character.IsCharacterGrounded())
//         {
//             // Toggle the control state
//             ContextEngine.ControlState newState = (ContextEngine.Instance.currentControlState == ContextEngine.ControlState.Character) 
//                 ? ContextEngine.ControlState.Spaceship 
//                 : ContextEngine.ControlState.Character;

//             ContextEngine.Instance.SetControlState(newState);
//         }
//     }
// }