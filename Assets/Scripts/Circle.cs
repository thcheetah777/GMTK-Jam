using System;
using UnityEngine;

[Serializable]
public struct Circle
{
    public Vector2 position;
    public float radius;

    public Circle(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
}