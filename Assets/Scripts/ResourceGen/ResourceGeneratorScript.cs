using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleResourceGen : MonoBehaviour
{
    static public int numberOfResourceCircles = 20;
    public float minSize = 100f;
    public float maxSize = 500f;
    public Color[] possibleColors = { Color.black, Color.red, Color.blue };
    public Vector2 areaSize = new Vector2(100f, 100f);
    public GameObject circlePrefab;
    public Material lineMaterial;

    private void Start()
    {
        print("before: " + transform.position);
        //for debugging, to see the area of which circles may spawn
        //DrawAreaBox();

        print("after: " + transform.position);
        //need to add a function that restricts their spawning to the outside edge of the asteroid
        //GenerateCircles();

    }

    void GenerateCircles()
    {
        

        for (int i = 0; i < numberOfResourceCircles - 1; i++)
        {
            // Create a random size
            float randomSize = Random.Range(minSize, maxSize);

            // Create a random color from the array
            Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

            GameObject circle = Instantiate(circlePrefab);
            circle.transform.parent = transform;

            // Set the color of the SpriteRenderer
            SpriteRenderer spriteRenderer = circle.GetComponent<SpriteRenderer>();
            spriteRenderer.color = randomColor;
            spriteRenderer.sortingLayerName = "Foreground";

            // Set the local scale to create the circle of the desired size
            transform.localScale = new Vector3(randomSize, randomSize, 1f);

            // Randomly position the circle within the specified area
            float randomX = Random.Range(areaSize.x / 2f, areaSize.x / 2f);
            float randomY = Random.Range(areaSize.y / 2f, areaSize.y / 2f);

            transform.position = new Vector3(randomX, randomY, 0f);
            
        }
    }

    void DrawAreaBox()
    {
        GameObject box = new GameObject("AreaBox");
        LineRenderer lineRenderer = box.AddComponent<LineRenderer>();

        // Set the material for the line renderer
        lineRenderer.material = lineMaterial;

        // Define the four corners of the area box
        Vector3[] corners = new Vector3[]
        {
            new Vector3(-areaSize.x / 2f, -areaSize.y / 2f, 0f),
            new Vector3(areaSize.x / 2f, -areaSize.y / 2f, 0f),
            new Vector3(areaSize.x / 2f, areaSize.y / 2f, 0f),
            new Vector3(-areaSize.x / 2f, areaSize.y / 2f, 0f),
            new Vector3(-areaSize.x / 2f, -areaSize.y / 2f, 0f) // Close the loop
        };

        // Set the positions and loop the line renderer
        lineRenderer.positionCount = corners.Length;
        lineRenderer.SetPositions(corners);
        lineRenderer.loop = true;
        lineRenderer.sortingLayerName = "Foreground";
        box.transform.parent = transform;
    }
}
