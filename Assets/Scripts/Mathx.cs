using UnityEngine;

public static class Mathx
{
    /// <summary>
    /// Returns f rounded to the nearest multiple of d.
    /// </summary>
    public static float RoundToNearest(float f, float d) => d * Mathf.Round(f / d);

    /// <summary>
    /// Returns f rounded to the nearest multiple of d.
    /// </summary>
    public static int RoundToNearestInt(float f, int d) => d * Mathf.RoundToInt(f / d);

    /// <summary>
    /// Returns the largest multiple of d smaller than or equal to f.
    /// </summary>
    public static float FloorToNearest(float f, float d) => d * Mathf.Floor(f / d);

    /// <summary>
    /// Returns the largest multiple of d smaller than or equal to f.
    /// </summary>
    public static int FloorToNearestInt(float f, int d) => d * Mathf.FloorToInt(f / d);

    /// <summary>
    /// Returns the smallest multiple of d greater than or equal to f.
    /// </summary>
    public static float CeilToNearest(float f, float d) => d * Mathf.Ceil(f / d);

    /// <summary>
    /// Returns the smallest multiple of d greater than or equal to f.
    /// </summary>
    public static int CeilToNearestInt(float f, int d) => d * Mathf.CeilToInt(f / d);
}
