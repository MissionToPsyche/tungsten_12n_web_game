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

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] GameObject objectiveOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive;

    [SerializeField, ReadOnly] public Text objectiveField;

    public Objective[] objectives;

    [SerializeField, ReadOnly] private string currentObjectiveGUI;
    [SerializeField, ReadOnly] private string currentObjective;
    [SerializeField, ReadOnly] private int index;

    private void Start()
    {
        gameObject.AddComponent<SetupCommunicationModule>();
        gameObject.AddComponent<SetupHabitatModule>();
        gameObject.AddComponent<SetupDriller>();
        gameObject.AddComponent<SetupMiner>();
        gameObject.AddComponent<SetupRefiner>();
        gameObject.AddComponent<DiscoveredLocations>();
        gameObject.AddComponent<CollectResources>();
        objectives = new Objective[7];
        objectives[0] = gameObject.GetComponent<SetupCommunicationModule>();
        objectives[1] = gameObject.GetComponent<SetupHabitatModule>();
        objectives[2] = gameObject.GetComponent<SetupDriller>();
        objectives[3] = gameObject.GetComponent<SetupMiner>();
        objectives[4] = gameObject.GetComponent<SetupRefiner>();
        objectives[5] = gameObject.GetComponent<DiscoveredLocations>();
        objectives[6] = gameObject.GetComponent<CollectResources>();
        index = 0;

        objectiveOverlay.SetActive(false);
    }

    // -------------------------------------------------------------------

    void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerObjectiveOverlay()
    {
        isOverlayActive = !isOverlayActive;
        objectiveOverlay.SetActive(isOverlayActive);
    }

    // -------------------------------------------------------------------

    void OnGUI()
    {
        currentObjectiveGUI = "";
        index=0;

        foreach (Objective objective in objectives) {
            currentObjective = String.Format("\nObjective {0} : {1}\n", index, objective.DrawHud());
            currentObjectiveGUI += currentObjective;
            index++;
        }
        objectiveField.text = currentObjectiveGUI;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Objective objective in objectives) {
            if (objective.IsAchieved()) {
                objective.Progression();
                Destroy(objective);
            }
        }
    }
}

public abstract class Objective : MonoBehaviour
{
    public abstract bool IsAchieved();
    public abstract void Progression();
    public abstract string DrawHud();
}

public class SetupHabitatModule : Objective
{
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

public class SetupCommunicationModule : Objective
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
        return string.Format("Communication Module Progression {0}/{1}", progression, requiredProgression);
    }
}

public class SetupDriller : Objective
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

public class SetupRefiner : Objective
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

public class SetupMiner : Objective
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

public class DiscoveredLocations : Objective
{
    public int currentDiscoveries = 0;
    public int maxDiscoveries = 10;

    public override bool IsAchieved() {
        return (currentDiscoveries >= maxDiscoveries);
    }
    public override void Progression()
    {
        currentDiscoveries += 1;
    }
    public override string DrawHud()
    {
        return string.Format("Discovered Locations {0}/{1}", currentDiscoveries, maxDiscoveries);

    }
}

public class CollectResources : Objective
{
    public int currentResources = 0;
    public int requiredResources = 100;

    public override bool IsAchieved()
    {
        return (currentResources >= requiredResources);
    }
    public override void Progression()
    {
        currentResources += 5;
    }
    public override string DrawHud()
    {
        return string.Format("Collected Resources {0}/{1}", currentResources, requiredResources);
    }
}
