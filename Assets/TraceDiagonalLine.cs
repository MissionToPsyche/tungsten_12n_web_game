using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class TraceDiagonalLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public SpriteRenderer diagonalLineSprite; // Reference to the sprite representing the diagonal line

    private bool isDrawing = false; // Flag to track if the user is currently drawing

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if the user starts drawing
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the mouse is inside the sprite
            if (IsInsideSprite(mousePosition))
            {
                isDrawing = true;
                UpdateLine(mousePosition);
            }
        }
        else if (Input.GetMouseButton(0) && isDrawing) // Check if the user is continuing to draw
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateLine(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)) // Check if the user stops drawing
        {
            isDrawing = false;
        }
    }

    private bool IsInsideSprite(Vector2 point)
    {
        // Check if the point is inside the sprite's bounds
        return diagonalLineSprite.bounds.Contains(point);
    }

    private void UpdateLine(Vector2 point)
    {
        // Update Line Renderer points here
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }
}
