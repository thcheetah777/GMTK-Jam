using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleFieldCollider))]
public class Trap : PausableMonoBehaviour
{
    private static List<Trap> All;

    public static int Damage = 4;

    /// <summary>
    /// Checks whether a given position is within the given radius of any trao.
    /// </summary>
    public static bool Check(Vector2 position, float radius) => All.Any(trap => trap.collider.Check(position, radius));
    /// <summary>
    /// Checks whether a given circle overlaps any trap.
    /// </summary>
    public static bool Check(Circle circle) => All.Any(trap => trap.collider.Check(circle));
    /// <summary>
    /// Checks whether a given point is within any trap.
    /// </summary>
    public static bool Check(Vector2 position) => All.Any(trap => trap.collider.Check(position));
    /// <summary>
    /// Checks whether a given circle collider overlaps any trap.
    /// </summary>
    public static bool Check(CircleCollider2D collider) => All.Any(trap => trap.collider.Check(collider));

    private new CircleFieldCollider collider;

    static Trap()
    {
        All = new();
        SceneManager.activeSceneChanged += (oldScene, newScene) => All.Clear();
    }

    private void Awake()
    {
        collider = GetComponent<CircleFieldCollider>();

        All.Add(this);
    }

    private void OnDestroy()
    {
        if (All.Count != 0)
            All.Clear();
    }
}
