using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof (MapGen))]

public class NoiseMapDebugger : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGen = (MapGen)target;

        if (DrawDefaultInspector())//for changing width and scales and updating in real time
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }

}
#endif