using UnityEngine;

public class GravityAreaCenter2D : GravityArea2D
{
    public override Vector2 GetGravityDirection(GravityBody2D gravityBody)
    {
        if (gravityBody == null) {
            return new Vector3(1,1,1); 
        }
        return (transform.position - gravityBody.transform.position).normalized;
    }
}
