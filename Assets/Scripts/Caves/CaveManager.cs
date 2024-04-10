using System.Collections;
using UnityEngine;

public class CaveManager : MonoBehaviour
{
    public static CaveManager instance { get; private set; }

    // [Header("Events")]

    // [Header("Mutable")]

    // [Header("ReadOnly")]

    // Not for display


    // -------------------------------------------------------------------
    // Handle events



    // -------------------------------------------------------------------
    // API



    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
