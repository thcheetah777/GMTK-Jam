using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleFieldCollider : PausableMonoBehaviour
{
    public List<Circle> circles;

    /// <summary>
    /// Checks whether a given position is within the given radius of the circle field.
    /// </summary>
    public bool Check(Vector2 position, float radius) => circles.Any(circle => (circle.position - position).sqrMagnitude <= (radius + circle.radius) * (radius + circle.radius));
    /// <summary>
    /// Checks whether a given circle overlaps the circle field.
    /// </summary>
    public bool Check(Circle other) => Check(other.position, other.radius);
    /// <summary>
    /// Checks whether a given point is within the circle field.
    /// </summary>
    public bool Check(Vector2 position) => Check(position, 0);
    /// <summary>
    /// Checks whether a given circle collider overlaps the circle field.
    /// </summary>
    public bool Check(CircleCollider2D collider) => Check(collider.transform.position, collider.radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y, collider.transform.lossyScale.z));
}
