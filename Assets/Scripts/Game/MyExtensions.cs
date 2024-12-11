using System.Collections;
using UnityEngine;

public static class MyExtensions
{

    public static Vector3 convertTo3DMovement(this Vector2 movement2D)
    {
        return new Vector3(movement2D.x, 0, movement2D.y);
    }
}