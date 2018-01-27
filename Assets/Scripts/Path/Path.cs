using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PointType
{
    Handle,
    Point
}

[System.Serializable]
public class Point
{
    public Vector2 Position;
    public PointType Type;
    public Point(Vector2 pos, PointType type)
    {
        this.Position = pos;
        this.Type = type;
    }
}

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Point> points;

    public Path(Vector2 centre)
    {
        points = new List<Point>
        {
            new Point (centre+Vector2.left , PointType.Point),
            new Point (centre+(Vector2.left+Vector2.up)*.5f, PointType.Handle),
            new Point (centre + (Vector2.right+Vector2.down)*.5f, PointType.Handle),
            new Point (centre + Vector2.right, PointType.Point)
        };
    }
    
    public Point this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return (points.Count - 4) / 3 + 1;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(new Point(points[points.Count - 1].Position * 2 - points[points.Count - 2].Position, PointType.Handle));
        points.Add(new Point((points[points.Count - 1].Position + anchorPos) * .5f, PointType.Handle));
        points.Add(new Point(anchorPos, PointType.Point));
    }

    public Vector2[] GetPointPositionsInSegment(int i)
    {
        return new Vector2[] { points[i * 3].Position, points[i * 3 + 1].Position, points[i * 3 + 2].Position, points[i * 3 + 3].Position };
    }

    public Point[] GetPointsInSegment(int i)
    {
        return new Point[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]};
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i].Position;
        points[i].Position = pos;

        if (i % 3 == 0)
        {
            if (i + 1 < points.Count)
            {
                points[i + 1].Position += deltaMove;
            }
            if (i - 1 >= 0)
            {
                points[i - 1].Position += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
            {
                float dst = (points[anchorIndex].Position - points[correspondingControlIndex].Position).magnitude;
                Vector2 dir = (points[anchorIndex].Position - pos).normalized;
                points[correspondingControlIndex].Position = points[anchorIndex].Position + dir * dst;
            }
        }
    }

}