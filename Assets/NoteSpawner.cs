using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class NoteSpawner : MonoBehaviour
{

    public GameObject noteHitPoint;
    public Vector3 noteHitPosition
    {
        get { return m_noteHitPosition; }
        set { m_noteHitPosition = value; }
    }
    private Vector3 m_noteHitPosition;

    [Range(0, 0.1f)]
    public float PerfectHitRange;
    [Range(0, 0.25f)]
    public float OkayHitRange;
    [Range(0, 0.5f)]
    public float BadHitRange;

    public GameObject[] Notes;

    MultiPathManager manager;

    private void Start()
    {
        manager = GetComponent<MultiPathManager>();
    }

    private void OnDrawGizmos()
    {
        noteHitPoint.transform.position = Handles.PositionHandle(noteHitPoint.transform.position, noteHitPoint.transform.rotation);
    }

    public void Update()
    {
        if (Time.time % 2 == 0)
        {
            var startPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
            var endPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
            if (endPath == startPath)
            {
                while (endPath == startPath)
                    endPath = manager.GetPathAtIndex(Random.Range(0, manager.PathCount));
            }

            GameObject note = Notes[Random.Range(0, Notes.Length)];
            Note n = note.GetComponent<Note>();
            n.EntryPath = startPath.path;
            n.ExitPath = endPath.path;
            Instantiate(note, transform);
        }
    }

}

