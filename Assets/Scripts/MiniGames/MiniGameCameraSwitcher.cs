using System.Collections;
using UnityEngine;
using Cinemachine;

public class MiniGameCameraSwitcher : MonoBehaviour
{
    
    public CinemachineVirtualCamera playerCamera; // Variable to store the current active camera
    public CinemachineVirtualCamera miniGameCamera; // Variable to store the mini game camera

    // Method to switch to mini game camera
    public void SwitchToMiniGameCamera()
    {
        // Store the current active camera and switch to mini game camera
         playerCamera.Priority = 0;
        miniGameCamera.Priority = 100;
    }

    // Method to switch back from mini game camera
    public void SwitchBackFromMiniGameCamera()
    {
        // Switch back to the previously active camera
        playerCamera.Priority = 100;
        miniGameCamera.Priority = 0;
    }
}
