using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircleFieldCollider))]
public class CircleFieldEditor : Editor
{
    CircleFieldCollider circleFieldCollider;

    private void OnEnable() => circleFieldCollider = target as CircleFieldCollider;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Shift click on the scene to add circles to the field. Holding control enables snapping to the grid when editing or adding circles.", MessageType.Info);
        EditorGUILayout.Space();

        SerializedProperty circleProperty = serializedObject.FindProperty("circles");
        EditorGUILayout.PropertyField(circleProperty);
    }

    private void OnSceneGUI()
    {
        if (Event.current.shift)
        {
            HandleUtility.AddDefaultControl(0);
            if (Event.current.type == EventType.MouseDown)
            {
                Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Vector2 mouseGroundPosition = mouseRay.origin - mouseRay.origin.z * mouseRay.direction / mouseRay.direction.z;
                if (EditorSnapSettings.gridSnapEnabled && Event.current.control)
                    mouseGroundPosition = new Vector2(
                        Mathx.RoundToNearest(mouseGroundPosition.x, EditorSnapSettings.move.x),
                        Mathx.RoundToNearest(mouseGroundPosition.y, EditorSnapSettings.move.y)
                    );
                circleFieldCollider.circles.Add(new Circle(mouseGroundPosition, 5));
            }
        }

        for (int i = 0; i < circleFieldCollider.circles.Count; i++)
        {
            Undo.RecordObject(circleFieldCollider, "Edit Circle Field Collider");
            Vector2 positionHandle = Handles.FreeMoveHandle(circleFieldCollider.circles[i].position, Quaternion.identity, 0.25f, EditorUtils.MoveSnap(), Handles.DotHandleCap);
            Vector2 radiusHandle = Handles.FreeMoveHandle(positionHandle + circleFieldCollider.circles[i].radius * Vector2.right, Quaternion.identity, 0.25f, EditorUtils.MoveSnap(), Handles.DotHandleCap);
            float radius = (radiusHandle - positionHandle).x;
            circleFieldCollider.circles[i] = new Circle(positionHandle, radius);
            Handles.DrawWireDisc(positionHandle, Vector3.back, radius);
        }
    }
}
