using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSwitches : MonoBehaviour
{
    public GameObject[] lights;

    void Start()
    {
        // Call the method to randomize switches
        RandomizeLights(lights);
    }

    void RandomizeLights(GameObject[] lightsArray)
    {
        // Shuffle the array
        Shuffle(lightsArray);

        // Ensure at least two lights remain off
        EnsureAtLeastTwoOff(lightsArray);

        // Activate lights based on the randomized array
        for (int i = 0; i < lightsArray.Length; i++)
        {
            lightsArray[i].SetActive(!lightsArray[i].activeSelf); // Toggle each light's state
        }
    }

    void Shuffle(GameObject[] array)
    {
        // Create a random number generator
        System.Random rng = new System.Random();

        // Perform Fisher-Yates shuffle
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
    }

    void EnsureAtLeastTwoOff(GameObject[] array)
    {
        // Count the number of lights that are off
        int offCount = 0;
        foreach (GameObject light in array)
        {
            if (!light.activeSelf)
                offCount++;
        }

        // If less than two lights are off, make at least two off
        if (offCount < 2)
        {
            for (int i = 0; i < array.Length && offCount < 2; i++)
            {
                if (array[i].activeSelf)
                {
                    array[i].SetActive(false);
                    offCount++;
                }
            }
        }
    }
}
