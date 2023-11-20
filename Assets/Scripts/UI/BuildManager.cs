using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    GameObject drawingPanel; 

    public GameObject prefab;

    void Start(){
        drawingPanel.SetActive(false);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B)) {
            if (drawingPanel.activeSelf)
            {
                drawingPanel.SetActive(false);
            }
            else 
            {
                drawingPanel.SetActive(true);
            }
        }
    }

    public void SpawnNewEntity()
    {
        Vector3 screenPos = new Vector3(375, 285, 10f); 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
        drawingPanel.SetActive(false);
    }
}
