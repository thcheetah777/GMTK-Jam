using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private static List<Trap> All;

    /// <summary>
    /// Checks whether a given position is within the given radius of any trao.
    /// </summary>
    public static bool Check(Vector2 position, float radius) => All.TrueForAll(trap => trap.collider.Check(position, radius));
    /// <summary>
    /// Checks whether a given circle overlaps any trap.
    /// </summary>
    public static bool Check(Circle circle) => All.TrueForAll(trap => trap.collider.Check(circle));
    /// <summary>
    /// Checks whether a given point is within any trap.
    /// </summary>
    public static bool Check(Vector2 position) => All.TrueForAll(trap => trap.collider.Check(position));
    /// <summary>
    /// Checks whether a given circle collider overlaps any trap.
    /// </summary>
    public static bool Check(CircleCollider2D collider) => All.TrueForAll(trap => trap.collider.Check(collider));

    private new CircleFieldCollider collider;

    private void Awake()
    {
        collider = GetComponent<CircleFieldCollider>();

        if (All == null)
            All = new();
        All.Add(this);
    }

    private void OnDestroy()
    {
        if (All != null)
            All = null;
    }
}
