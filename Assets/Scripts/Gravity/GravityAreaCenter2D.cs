using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityAreaCenter2D : GravityArea2D
{
    public override Vector2 GetGravityDirection(GravityBody2D gravityBody)
    {
        return (transform.position - gravityBody.transform.position).normalized;
    }
}
