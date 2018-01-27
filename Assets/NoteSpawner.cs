using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class NoteSpawner : MonoBehaviour {

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

    public Transform noteATransform;
    //public Transform noteBTransform;
    
    private float pos = 0;

    //public Vector3[] PathPoints;
    public PathCreator p;
    public PathCreator p2;
    Vector2[] points;
    int segment = 0;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        noteHitPoint.transform.position = Handles.PositionHandle(noteHitPoint.transform.position, noteHitPoint.transform.rotation);
    }
#endif

    public void UpdateNoteHitPosition()
    {
        noteHitPoint.transform.position = m_noteHitPosition;
    }

    Dictionary<int, float> segmentDistances = new Dictionary<int, float>();
    //float properTime;
    //float normalisedTime;
    float totalLength = 0;

    void PrecalcDistances ()
    {
        totalLength = PathCreator.ApproxBezierLength(p.path);

        for (int i = 0; i < p.path.NumSegments; i++)
        {
            var pointsOnPath = p.path.GetPointPositionsInSegment(i);
            float segmentLength = PathCreator.ApproxSegmentLength(pointsOnPath[0], pointsOnPath[1], pointsOnPath[2], pointsOnPath[3]);
            segmentDistances[i] = segmentLength;
        }

        //Debug.Log("Addition of distances: " + segmentDistances.Select(x => x.Value).Sum() + " vs Total Length: " + PathCreator.ApproxBezierLength(p.path));

        //Debug.Log("First segment takes up: " + (segmentDistances[0] / PathCreator.ApproxBezierLength(p.path)) * 100);
        //Debug.Log("Second segment takes up: " + (segmentDistances[1] / PathCreator.ApproxBezierLength(p.path)) * 100);
        //Debug.Log("Time to traverse first segment: " + (1.5f / 100) * ((segmentDistances[0] / PathCreator.ApproxBezierLength(p.path)) * 100));
        //Debug.Log("Time to traverse second segment: " + (1.5f / 100) * ((segmentDistances[1] / PathCreator.ApproxBezierLength(p.path)) * 100));
    }

    private void Start()
    {
        PrecalcDistances();
    }

    PathCreator.Direction noteDirectio = PathCreator.Direction.Forward;
    // Update is called once per frame
    void Update() {
        
        if (pos <= 1)
        {
            pos += Time.deltaTime;
            noteATransform.position = PathCreator.PointOnLine(p.path, pos, 1.5f, noteDirectio);
        }
        else
        {
            p = p2;
            noteDirectio = PathCreator.Direction.Backward;
            pos = 0;
        }
    }
}

