using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveSceneManager : MonoBehaviour
{
    public void loadAsteroidScene() 
    {
        SceneManager.LoadScene("AsteroidScene");
    }
    public void loadCaveScene(String caveScene)
    {
        SceneManager.LoadScene(caveScene);
    }

    public void loadCaveScene1() {
        SceneManager.LoadScene("Cave_1");
    }
    
    public void loadCaveScene2() {
        SceneManager.LoadScene("Cave_2");
    }
    
    public void loadCaveScene3() {
        SceneManager.LoadScene("Cave_3");
    }
    
    public void loadCaveScene4() {
        SceneManager.LoadScene("Cave_4");
    }
    
    public void loadCaveScene5() {
        SceneManager.LoadScene("Cave_5");
    }
    
    public void loadCaveScene6() {
        SceneManager.LoadScene("Cave_6");
    }
    public void loadCaveScene7() {
        SceneManager.LoadScene("Cave_7");
    }
    
    public void loadCaveScene8() {
        SceneManager.LoadScene("Cave_8");
    }
    public void loadCaveScene9() {
        SceneManager.LoadScene("Cave_9");
    }
    
    public void loadCaveScene10() {
        SceneManager.LoadScene("Cave_10");
    }
}
