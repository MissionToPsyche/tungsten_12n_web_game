using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject up;
    public GameObject on;
    public bool isOn;
    public bool isUp;
    void Start()
    {
    // Randomize isOn and isUp values
        isOn = Random.value > 0.4f; // 40% chance of being true or false
        isUp = Random.value > 0.5f; // 50% chance of being true or false

    // Set the active state of the GameObjects based on the randomized values
        on.SetActive(isOn);
        up.SetActive(isUp);

    // If the switch starts in the "on" position, notify the Main instance
        
    }


    private void OnMouseUp(){
        isUp = !isUp;
        isOn = !isOn;
        on.SetActive(isOn);
        up.SetActive(isUp); 
        
    }

    // Update is called once per frame
   
}
