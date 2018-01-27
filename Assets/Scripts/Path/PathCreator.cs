using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PathCreator : MonoBehaviour
{

    [HideInInspector]
    public Path path;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * a;
    }

    public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 lerp0 = Lerp(a, b, t);
        Vector2 lerp1 = Lerp(b, c, t);
        return Lerp(lerp0, lerp1, t);
    }

    public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        return (Mathf.Pow((1 - t), 3) * a) 
            + (3 * (Mathf.Pow((1 - t), 2) * t * b)) 
            + (3 * (1 - t) * (Mathf.Pow(t, 2) * c)) 
            + (Mathf.Pow(t, 3) * d);
    }

    public enum Direction
    {
        Forward,
        Backward
    }

    private static int GetSegmentForPoint(Path path, float t, ref float time_in_seg, float totalPathLength)
    {
        float curLength = 0;

        for (int i = 0; i < path.NumSegments; i++)
        {
            var pointInSegment = path.GetPointPositionsInSegment(i);
            curLength += ApproxSegmentLength(pointInSegment[0], pointInSegment[1], pointInSegment[2], pointInSegment[3]);
            //Debug.Log("Position on line: " + t);
            if (t < (curLength / totalPathLength))
            {
                Debug.Log("Segment we're returning: " + i);
                
                Debug.Log("t: " + t + " | curLenght: " + curLength + " | totalPathLength: "+ totalPathLength);
                time_in_seg = Mathf.Abs(t - ((curLength - ApproxSegmentLength(pointInSegment[0], pointInSegment[1], pointInSegment[2], pointInSegment[3])) / totalPathLength));
                Mathf.Clamp01(time_in_seg);
                Debug.Log("Time in seg: " + time_in_seg);

                return i;
            }
        }

        //Debug.Log("Position on line: " + t);
        return path.NumSegments;
    }

    public static Vector2 PointOnLine(Path path, float t, float totalTimeToTraverse, Direction direction)
    {
        float totalLength = ApproxBezierLength(path);
        float totalSpeed = (totalLength / totalTimeToTraverse);
        float amountAlongPath = direction == Direction.Forward ? t : 1 - t;
        float amountAlongSegment = 0;
        int currentSegment = GetSegmentForPoint(path, amountAlongPath, ref amountAlongSegment, totalLength);

        if (currentSegment == path.NumSegments)
        {
            var pointInSegment = path.GetPointPositionsInSegment(currentSegment - 1);
            return CubicCurve(pointInSegment[0], pointInSegment[1], pointInSegment[2], pointInSegment[3], 1);
        }
        else
        {
            var pointInSegment = path.GetPointPositionsInSegment(currentSegment);
            float segmentLength = ApproxSegmentLength(pointInSegment[0], pointInSegment[1], pointInSegment[2], pointInSegment[3]);

            //Debug.Log("TotalSpeed: " + totalSpeed + " | SegmentLength: " + segmentLength + " | TotalTimeToTraverse: " + totalTimeToTraverse + " | AmountAlongPath: " + amountAlongSegment);
            //Debug.Log("Total Calculated Value: " + (totalSpeed / (segmentLength / totalTimeToTraverse)) * amountAlongSegment);
            return CubicCurve(pointInSegment[0], pointInSegment[1], pointInSegment[2], pointInSegment[3], (totalSpeed / (segmentLength / totalTimeToTraverse)) * amountAlongSegment);
        }
    }

    public static float SpeedOverPath(Path p, float time)
    {
        float totalLength = ApproxBezierLength(p);
        return totalLength / time;

        //float percentage = t * 100;
        //float currentLengthCalc = 0;

        //Vector2[] segmentPoints = new Vector2[4];
        //float segmentLength;

        //float amountIntoSegment = 0;

        //for (int i = 0; i < p.NumSegments; i++)
        //{
        //    var points = p.GetPointPositionsInSegment(i);
        //    float currentSegmentLength = ApproxSegmentLength(points[0], points[1], points[2], points[3]);
        //    currentLengthCalc += currentSegmentLength;
        //    if (percentage < (currentLengthCalc / totalLength) * 100)
        //    {
        //        //we must lie between the point i and the previous point
        //        segmentPoints = points;
        //        segmentLength = currentSegmentLength;
        //        amountIntoSegment = (percentage - ((currentLengthCalc / totalLength) * 100)) / 100;
        //        break;
        //    }
        //}

        //return CubicCurve(segmentPoints[0], segmentPoints[1], segmentPoints[2], segmentPoints[3], amountIntoSegment);

        //if (segment < p.path.NumSegments)
        //{
        //    points = p.path.GetPointsInSegment(segment);
        //    noteTransform.position = PathCreator.CubicCurve(points[0], points[1], points[2], points[3], pos);
        //    pos += Time.deltaTime / PathCreator.ApproxLength(points[0], points[1], points[2], points[3]) * Speed;

        //    if (pos > 1)
        //    {
        //        pos = 0;
        //        segment++;
        //    }
        //}
    }

    public static float ApproxSegmentLength(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        float chord = (d - a).magnitude;
        float cont_net = (a - b).magnitude + (c - b).magnitude + (d - c).magnitude;
        return (cont_net + chord) / 2;
    }

    public static float ApproxBezierLength(Path p)
    {
        float length = 0;
        for (int i = 0; i < p.NumSegments; i++)
        {
            var points = p.GetPointPositionsInSegment(i);
            length += ApproxSegmentLength(points[0], points[1], points[2], points[3]);
        }
        return length;
    }

    private void OnDrawGizmos()
    {
        if (path == null || path.NumPoints == 0)
            return;

        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointPositionsInSegment(i);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }
    }
}