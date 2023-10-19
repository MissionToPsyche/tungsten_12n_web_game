using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateResource : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 10.0f;
    public int stepCount = 4; // Number of steps

    // Start is called before the first frame update
    void Start()
    {
        GeneratePerlinNoise();
    }

    

    void GeneratePerlinNoise()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sharedMaterial.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                // Apply the step function
                float steppedValue = Mathf.Floor(sample * stepCount) / stepCount;

                // Convert to color
                Color color = new Color(steppedValue, steppedValue, steppedValue);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

}
