using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;

public class GoalManager : MonoBehaviour {
    [SerializeField]
    GameObject objPanel; 
    [SerializeField]
    public Text objectiveField;
    public Goal[] goals;

    private string currGoalGUI; 
    private string currGoal; 
    private int index; 
    void Start() {
        gameObject.AddComponent<SetupCommunicationModule>(); 
        gameObject.AddComponent<SetupHabitatModule>();
        gameObject.AddComponent<SetupDriller>();
        gameObject.AddComponent<SetupMiner>();
        gameObject.AddComponent<SetupRefiner>();
        gameObject.AddComponent<DiscoveredLocations>(); 
        gameObject.AddComponent<CollectResources>(); 
        goals = new Goal[7];
        goals[0] = gameObject.GetComponent<SetupCommunicationModule>();
        goals[1] = gameObject.GetComponent<SetupHabitatModule>();
        goals[2] = gameObject.GetComponent<SetupDriller>();
        goals[3] = gameObject.GetComponent<SetupMiner>();
        goals[4] = gameObject.GetComponent<SetupRefiner>();
        goals[5] = gameObject.GetComponent<DiscoveredLocations>(); 
        goals[6] = gameObject.GetComponent<CollectResources>(); 
        index = 0; 
        objPanel.SetActive(false);
    }


    void OnGUI() {
        currGoalGUI = "";  
        index=0; 

        foreach (Goal goal in goals) {
            currGoal = String.Format("\nObjective {0} : {1}\n", index, goal.DrawHud());
            currGoalGUI = currGoalGUI + currGoal;
            index++;
        }
        objectiveField.text = currGoalGUI;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Goal goal in goals) {
            if (goal.IsAchieved()) {
                goal.Progression(); 
                Destroy(goal);
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {

            if (objPanel.activeSelf)
            {
                objPanel.SetActive(false);
            }
            else 
            {
                objPanel.SetActive(true);
            }
        }
    }
}

public abstract class Goal : MonoBehaviour {
    public abstract bool IsAchieved();
    public abstract void Progression();
    public abstract string DrawHud(); 
}

public class SetupHabitatModule : Goal {
    public int progression = 0;
    public int requiredProgression = 100; 

    public override bool IsAchieved() {
        return (progression >= requiredProgression); 
    }
    public override void Progression() {
        progression += 15; 
    }
    public override string DrawHud() {
        return string.Format("Habitat Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class SetupCommunicationModule : Goal {
    public int progression = 0; 
    public int requiredProgression = 100;

    public override bool IsAchieved()
    {
        return (progression >= requiredProgression);
    }
    public override void Progression()
    {
        progression += 10; 
    }
    public override string DrawHud()
    {
        return string.Format("Communication Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class SetupDriller : Goal 
{
    public int progression = 0; 
    public int requiredProgression = 100;

    public override bool IsAchieved()
    {
        return (progression >= requiredProgression);
    }
    public override void Progression()
    {
        progression += 10; 
    }
    public override string DrawHud()
    {
        return string.Format("Driller Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class SetupRefiner : Goal 
{
    public int progression = 0; 
    public int requiredProgression = 100;

    public override bool IsAchieved()
    {
        return (progression >= requiredProgression);
    }
    public override void Progression()
    {
        progression += 10; 
    }
    public override string DrawHud()
    {
        return string.Format("Refiner Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class SetupMiner : Goal 
{
    public int progression = 0; 
    public int requiredProgression = 100;

    public override bool IsAchieved()
    {
        return (progression >= requiredProgression);
    }
    public override void Progression()
    {
        progression += 10; 
    }
    public override string DrawHud()
    {
        return string.Format("SetupMiner Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class DiscoveredLocations : Goal {
    public int currDiscoveries = 0; 
    public int maxDiscoveries = 10; 

    public override bool IsAchieved() {
        return (currDiscoveries >= maxDiscoveries);
    }
    public override void Progression()
    {
        currDiscoveries += 1; 
    }
    public override string DrawHud()
    {
        return string.Format("Discovered Locations {0}/{1}", currDiscoveries, maxDiscoveries);
        
    }
}

public class CollectResources : Goal {
    public int currResources = 0; 
    public int requiredResources = 100;

    public override bool IsAchieved()
    {
        return (currResources >= requiredResources);
    }
    public override void Progression()
    {
        currResources += 5; 
    }
    public override string DrawHud()
    {
        return string.Format("Collected Resources {0}/{1}", currResources, requiredResources);
    }
}