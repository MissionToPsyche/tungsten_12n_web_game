using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField] private GameObject prefab;

    [MenuItem("Tools/Replace With Prefab")]
    static void CreateReplaceWithPrefab()
    {
        EditorWindow.GetWindow<ReplaceWithPrefab>();
    }

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            foreach (var selected in selection)
            {
                if (prefab == null)
                {
                    Debug.LogError("Prefab is not assigned.");
                    return;
                }

                GameObject newObject;

                // Check if the prefab is an asset or an instance
                if (PrefabUtility.GetPrefabAssetType(prefab) != PrefabAssetType.NotAPrefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                // Preserve the SpriteRenderer component from the original GameObject
                var originalRenderer = selected.GetComponent<SpriteRenderer>();
                if (originalRenderer != null)
                {
                    var newRenderer = newObject.GetComponent<SpriteRenderer>();
                    if (newRenderer == null)
                    {
                        newRenderer = newObject.AddComponent<SpriteRenderer>();
                    }
                    newRenderer.sprite = originalRenderer.sprite;
                    newRenderer.color = originalRenderer.color;
                    newRenderer.flipX = originalRenderer.flipX;
                    newRenderer.flipY = originalRenderer.flipY;
                    newRenderer.maskInteraction = originalRenderer.maskInteraction;
                    newRenderer.receiveShadows = originalRenderer.receiveShadows;
                    newRenderer.shadowCastingMode = originalRenderer.shadowCastingMode;
                    newRenderer.sharedMaterial = originalRenderer.sharedMaterial;
                    newRenderer.sharedMaterials = originalRenderer.sharedMaterials;
                    newRenderer.sortingLayerID = originalRenderer.sortingLayerID;
                    newRenderer.sortingLayerName = originalRenderer.sortingLayerName;
                    newRenderer.sortingOrder = originalRenderer.sortingOrder;
                }

                // Copy serialized fields from selected GameObject to the new prefab instance
                var fields = selected.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
                    {
                        field.SetValue(newObject.GetComponent(field.DeclaringType), field.GetValue(selected.GetComponent(field.DeclaringType)));
                    }
                }

                // Register the new object for undo
                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");

                // Transfer the transform from the selected GameObject to the new prefab instance
                newObject.transform.SetParent(selected.transform.parent);
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());

                // Destroy the selected GameObject
                Undo.DestroyObjectImmediate(selected);
            }
        }

        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}
