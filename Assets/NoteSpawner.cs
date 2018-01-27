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

    public float Speed = 10f;
    private float pos = 0;

    //public Vector3[] PathPoints;
    public PathCreator p;
    Vector2[] points;
    int segment = 0;

    private void OnDrawGizmos()
    {
        noteHitPoint.transform.position = Handles.PositionHandle(noteHitPoint.transform.position, noteHitPoint.transform.rotation);
    }

    public void UpdateNoteHitPosition()
    {
        noteHitPoint.transform.position = m_noteHitPosition;
    }

    // Update is called once per frame
    void Update () {

        if (p == null)
            return;

        if (segment < p.path.NumSegments)
        {
            points = p.path.GetPointPositionsInSegment(segment);
            noteATransform.position = PathCreator.CubicCurve(points[0], points[1], points[2], points[3], pos);
            pos += Time.deltaTime / PathCreator.ApproxSegmentLength(points[0], points[1], points[2], points[3]) * Speed;

            if (pos > 1)
            {
                pos = 0;
                segment++;
            }
        }

        ////noteTransform.position = BezierCurve.GetQuadraticCurvePoint(bz.GetPointAt(0), bz.GetPointAt(1), bz.GetPointAt(2), 0.5f);
        //var arr = bz.GetAnchorPoints();
        //List<Vector3> vc = new List<Vector3>();
        //foreach (var bzPoint in arr)
        //    vc.Add(bzPoint.transform.position);
        //vc.Reverse();

        //bz.length;

        //if (curBezierPoint < points.Length - 1)
        //{
        //    float approxLength = BezierCurve.ApproximateLength(points[curBezierPoint], points[curBezierPoint + 1]);
        //    noteTransform.position = BezierCurve.GetPoint(points[curBezierPoint], points[curBezierPoint + 1], pos);
        //    pos += Time.deltaTime / (approxLength * damping);

        //    if (pos >= 1)
        //    {
        //        pos = 0;
        //        curBezierPoint++;
        //    }
        //}
    }
    
    void MovePoint()
    {
        
    }
}
