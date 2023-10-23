using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoint : MonoBehaviour
{
    public float gravityScale = 10.0f;           // A multiplier to define the strength of the gravity.
    public CircleCollider2D innerCollider;     // Represents where gravity is at its maximum.
    public CircleCollider2D outerCollider;     // Represents the outer boundary of the gravity effect.

    public Vector3 gravityDirection { get; private set; }
    private float gravitationalPower = 0f;

    public delegate void GravitationalEffectHandler(Vector3 gravityDirection, float gravitationalPower);
    public event GravitationalEffectHandler OnGravityEffect;



    // Awake is called when the script instance is being loaded.
    void Awake()
    {
    }

    // OnTriggerStay2D is called once per frame for every Collider2D object that is touching the trigger.
    void OnTriggerStay2D(Collider2D obj)
    {
        float effectiveInnerRadius = innerCollider.radius * innerCollider.transform.localScale.x; // assuming x-scale is representative
        float effectiveOuterRadius = outerCollider.radius * outerCollider.transform.localScale.x; // same assumption

        float dist = Vector2.Distance(obj.transform.position, transform.position);

        // Debug.Log($"Player Position: {obj.transform.position}");
        // Debug.Log($"Gravity Center Position: {transform.position}");

        // Debug to show the distance of the object from the gravity source.
        // Debug.Log($"Distance from gravity source: {dist}");

        // Calculate gravitational power based on the distance from the source.
        if (dist <= effectiveInnerRadius)
        {
            gravitationalPower = gravityScale;
        }
        else
        {
            gravitationalPower = Mathf.Max(0, gravityScale * ((effectiveOuterRadius - dist) / (effectiveOuterRadius - effectiveInnerRadius + 1)));
        }

        // Debug.Log($"Inner Collider Radius: {effectiveInnerRadius}");
        // Debug.Log($"Outer Collider Radius: {effectiveOuterRadius}");

        // // Debug to display the calculated gravitational power.
        // Debug.Log($"Gravitational Power: {gravitationalPower}");

        // Calculate the direction of the gravity force.
        gravityDirection = (transform.position - obj.transform.position).normalized;

        // Debug to show the direction of the gravity.
        // Debug.Log($"Gravity Direction: {gravityDirection}");

        // Apply the calculated gravity force to the object's Rigidbody2D.
        obj.GetComponent<Rigidbody2D>().AddForce(gravityDirection * gravitationalPower);

        OnGravityEffect?.Invoke(gravityDirection, gravitationalPower);
    }
}
