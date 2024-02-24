using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour
{
    public GameObject[] lights;

    void Update()
    {
        // Check if all lights are turned on
        bool allLightsOn = CheckAllLightsOn();

        if (allLightsOn)
        {
            Debug.Log("You win!");
            // Implement your win condition actions here
        }
    }

    bool CheckAllLightsOn()
    {
        foreach (GameObject light in lights)
        {
            if (light != null && !light.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
}
