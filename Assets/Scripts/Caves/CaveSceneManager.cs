using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveSceneManager : MonoBehaviour
{
    public void loadAsteroidScene() 
    {
        SceneManager.LoadScene("AsteroidScene");
    }
    public void loadCaveScene() {
        SceneManager.LoadScene("Cave_9");
    }
}
