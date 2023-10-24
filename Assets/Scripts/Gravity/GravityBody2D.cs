using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody2D : MonoBehaviour
{
    private const float GravityForce = 800;

    public Vector2 GravityDirection
    {
        get
        {
            if (gravityAreas.Count == 0) return Vector2.zero;
            gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody2D objectBody2D;
    private List<GravityArea2D> gravityAreas;

    private void Start()
    {
        objectBody2D = GetComponent<Rigidbody2D>();
        gravityAreas = new List<GravityArea2D>();
    }

    private void FixedUpdate()
    {
        objectBody2D.AddForce(GravityDirection * (GravityForce * Time.fixedDeltaTime), ForceMode2D.Force);

        float rotationAngle = Vector2.SignedAngle(transform.up, -GravityDirection);
        float newRotation = Mathf.LerpAngle(objectBody2D.rotation, objectBody2D.rotation + rotationAngle, Time.fixedDeltaTime * 3f);
        objectBody2D.MoveRotation(newRotation);
    }

    public delegate void GravityAreaChangeHandler();
    public event GravityAreaChangeHandler OnEnterGravityArea;
    public event GravityAreaChangeHandler OnExitGravityArea;

    public void AddGravityArea(GravityArea2D gravityArea)
    {
        gravityAreas.Add(gravityArea);
        OnEnterGravityArea?.Invoke();
    }

    public void RemoveGravityArea(GravityArea2D gravityArea)
    {
        if (gravityAreas.Count == 0)
        {
            OnExitGravityArea?.Invoke();
        }
    }
}