using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoteSpawner))]
public class NoteSpawnerEditor : Editor {

    private NoteSpawner ns;

    public void OnSceneGUI()
    {
        if (ns == null)
            ns = this.target as NoteSpawner;

        Handles.color = Color.red;
        //Handles.CircleHandleCap(0, ns.noteHitPoint.transform.position, Quaternion.identity, ns.BadHitRange, EventType.Ignore);
        Handles.DrawWireDisc(ns.noteHitPoint.transform.position, ns.noteHitPoint.transform.forward, ns.BadHitRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(ns.noteHitPoint.transform.position, ns.noteHitPoint.transform.forward, ns.OkayHitRange);
        Handles.color = Color.green;
        Handles.DrawWireDisc(ns.noteHitPoint.transform.position, ns.noteHitPoint.transform.forward, ns.PerfectHitRange);

        if (Selection.activeGameObject == ns.gameObject)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(ns.noteHitPoint.transform.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(ns, "Change Position");
                ns.noteHitPosition = newTargetPosition;
                ns.UpdateNoteHitPosition();
            }
        }
        else
        {
            
        }
    }
}
