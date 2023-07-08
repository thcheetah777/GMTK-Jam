using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorUtils
{
    public static Vector3 MoveSnap() => (EditorSnapSettings.gridSnapEnabled && Event.current.control) ? EditorSnapSettings.move : Vector3.zero;
    public static float RotateSnap() => (EditorSnapSettings.gridSnapEnabled && Event.current.control) ? EditorSnapSettings.rotate : 0;
    public static float ScaleSnap() => (EditorSnapSettings.gridSnapEnabled && Event.current.control) ? EditorSnapSettings.scale : 0;
}
