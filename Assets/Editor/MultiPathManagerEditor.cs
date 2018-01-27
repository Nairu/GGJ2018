using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiPathManager))]
public class MultiPathManagerEditor : Editor {

    MultiPathManager manager;
    SerializedProperty points;

    private static bool showPoints = true;

    public void OnEnable()
    {
        manager = target as MultiPathManager;
        points = serializedObject.FindProperty("paths");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (manager == null)
            manager = target as MultiPathManager;


        showPoints = EditorGUILayout.Foldout(showPoints, "Paths");

        if (showPoints)
        {
            if (points != null)
            {
                for (int i = 0; i < points.arraySize; i++)
                {
                    DrawPathEditor(manager.GetPathAtIndex(i), i);
                }
            }

            if (GUILayout.Button("Add Path"))
            {
                Undo.RegisterCompleteObjectUndo(manager, "Add Path");

                GameObject pathObject = new GameObject("Path" + points.arraySize);
                pathObject.transform.parent = manager.transform;
                pathObject.transform.localPosition = Vector3.zero;
                PathCreator pc = pathObject.AddComponent<PathCreator>();
                points.InsertArrayElementAtIndex(points.arraySize);
                points.GetArrayElementAtIndex(points.arraySize - 1).objectReferenceValue = pc;
                
                if (pc.path == null)
                {
                    pc.CreatePath();
                }
            }
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        //foreach (PathCreator pc in manager.paths)
        //{
        //    //draw a custom creation thing here
        //    for (int i = 0; i < pc.path.NumSegments; i++)
        //    {
        //        Vector2[] points = pc.path.GetPointsInSegment(i);
        //        Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        //    }
        //}
    }

    private void DrawPathEditor(PathCreator pathCreator, int index)
    {
        if (pathCreator == null)
        {
            manager.ResetPaths();
            return;
        }

        SerializedObject serObj = new SerializedObject(pathCreator.gameObject);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.ObjectField(pathCreator.gameObject, typeof(GameObject), true);

        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            Undo.RegisterCompleteObjectUndo(manager, "Remove Point");
            points.MoveArrayElement(manager.GetPathIndex(pathCreator), manager.PathCount - 1);
            points.arraySize--;
            DestroyImmediate(pathCreator.gameObject);
            return;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel++;
        EditorGUI.indentLevel++;

        for (int i = 0; i < pathCreator.path.NumSegments; i++)
        {
            EditorGUILayout.LabelField("Segment: " + i.ToString());
            EditorGUI.indentLevel++;
            foreach (Point p in pathCreator.path.GetPointsInSegment(i))
            {
                if (p.Type == PointType.Point)
                {
                    //draw the transform information in the window
                    p.Position = EditorGUILayout.Vector2Field("Position: ", p.Position);
                }
            }
            //if (GUILayout.Button("+", GUILayout.Width(20)))
            //{
            //    Undo.RegisterCompleteObjectUndo(pathCreator, "Add Point");
            //    pathCreator.path.AddSegment(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)));
            //}
            EditorGUI.indentLevel--;
        }

        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            serObj.ApplyModifiedProperties();
            EditorUtility.SetDirty(pathCreator);
        }
    }
}
