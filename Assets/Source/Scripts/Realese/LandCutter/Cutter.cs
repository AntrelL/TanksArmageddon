using System;
using System.Collections.Generic;
using System.Linq;
using TanksArmageddon;
using UnityEngine;

[Serializable]
public class Point
{
    public Vector2 Position;
    public Point NextPoint;
    public bool IsCross;
    public Segment CircleSegment;
    public Segment LandSegment;
}

public class Segment
{
    public Point A;
    public Point B;
    public List<Point> CrossPoints = new List<Point>();
}

[Serializable]
public class Line
{
    public List<Point> Points;
    public List<Segment> Segments;
}

public class Cutter : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _landCollider;
    [SerializeField] private PolygonCollider2D _circleCollider;
    [SerializeField] private int _testIterations = 10;

    public void DoCut()
    {
        var _circlePointsPositions = _circleCollider.GetPath(0).ToList();

        for (var i = 0; i < _circlePointsPositions.Count; i++)
            _circlePointsPositions[i] = _circleCollider.transform.TransformPoint(_circlePointsPositions[i]);

        var circleLine = LineFromCollider(_circlePointsPositions);


        var allSplines = new List<List<Point>>();

        for (var p = 0; p < _landCollider.pathCount; p++)
        {
            var _linePointsPositions = _landCollider.GetPath(p).ToList();

            for (var i = 0; i < _linePointsPositions.Count; i++)
                _linePointsPositions[i] = _landCollider.transform.TransformPoint(_linePointsPositions[i]);

            var landLine = LineFromCollider(_linePointsPositions);

            for (var i = 0; i < landLine.Points.Count; i++)
                if (_circleCollider.ClosestPoint(landLine.Points[0].Position) == landLine.Points[0].Position)
                {
                    ReorderList(landLine.Points);
                    ReorderList(landLine.Segments);
                }
                else
                {
                    break;
                }

            var result = Substraction(landLine, circleLine);
            allSplines.InsertRange(0, result);
        }

        _landCollider.GetComponent<Land>().SetPath(allSplines);
    }

    private List<List<Point>> Substraction(Line landLine, Line circleLine)
    {
        for (var i = 0; i < circleLine.Points.Count; i++)
        {
            var nextIndex = GetNext(i, circleLine.Points.Count, false);
            circleLine.Points[i].NextPoint = circleLine.Points[nextIndex];
        }

        for (var l = 0; l < landLine.Segments.Count; l++)
        {
            var landSegment = landLine.Segments[l];
            var al = landSegment.A.Position;
            var bl = landSegment.B.Position;

            for (var c = 0; c < circleLine.Segments.Count; c++)
            {
                var circleSegment = circleLine.Segments[c];
                var ac = circleLine.Segments[c].A.Position;
                var bc = circleLine.Segments[c].B.Position;

                if (Intersection.IsIntersecting(al, bl, ac, bc))
                {
                    var position = Intersection.GetIntersection(al, bl, ac, bc);
                    var crossPoint = new Point();
                    crossPoint.Position = position;
                    crossPoint.LandSegment = landSegment;
                    crossPoint.CircleSegment = circleSegment;
                    crossPoint.IsCross = true;
                    landSegment.CrossPoints.Add(crossPoint);
                    circleSegment.CrossPoints.Add(crossPoint);
                }
            }
        }

        RecalculateLine(landLine);
        RecalculateLine(circleLine);

        {
            var allPoints = new List<Point>(landLine.Points);
            var onLand = true;
            var startPoint = allPoints[0];

            while (allPoints.Count > 0)
            {
                var thePoint = allPoints[0];

                if (_circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                {
                    allPoints.RemoveAt(0);
                    continue;
                }

                for (var i = 0; i < _testIterations; i++)
                {
                    Line currentLine;

                    bool ccw;

                    if (onLand)
                    {
                        currentLine = landLine;
                        ccw = true;
                    }
                    else
                    {
                        currentLine = circleLine;
                        ccw = false;
                    }

                    var currentIndex = currentLine.Points.IndexOf(thePoint);
                    var nextIndex = GetNext(currentIndex, currentLine.Points.Count, ccw);
                    thePoint.NextPoint = currentLine.Points[nextIndex];
                    allPoints.Remove(thePoint);

                    if (thePoint.NextPoint.IsCross) onLand = !onLand;

                    thePoint = thePoint.NextPoint;

                    if (startPoint == thePoint) break;
                }
            }
        }

        {
            var allSplines = new List<List<Point>>();
            var allPoints = new List<Point>(landLine.Points);

            while (allPoints.Count > 0)
            {
                var thePoint = allPoints[0];

                if (_circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                {
                    allPoints.RemoveAt(0);
                }
                else
                {
                    var newSpline = new List<Point>();
                    allSplines.Add(newSpline);

                    var startPoint = thePoint;
                    var point = thePoint;

                    newSpline.Add(point);
                    allPoints.Remove(point);

                    for (var i = 0; i < _testIterations; i++)
                    {
                        point = point.NextPoint;
                        if (point == startPoint) break;
                        newSpline.Add(point);
                        if (allPoints.Contains(point))
                            allPoints.Remove(point);
                    }
                }
            }

            return allSplines;
        }
    }

    private void RecalculateLine(Line line)
    {
        var newPoints = new List<Point>();

        for (var s = 0; s < line.Segments.Count; s++)
        {
            var segment = line.Segments[s];
            newPoints.Add(segment.A);

            if (segment.CrossPoints.Count > 0)
                segment.CrossPoints.Sort((p1, p2) =>
                    Vector3.Distance(segment.A.Position, p1.Position)
                        .CompareTo(Vector3.Distance(segment.A.Position, p2.Position)));
            newPoints.AddRange(segment.CrossPoints);
        }

        line.Points = newPoints;
    }

    private void ReorderList<T>(List<T> list)
    {
        var first = list[0];

        for (var i = 0; i < list.Count; i++)
            if (i == list.Count - 1)
                list[i] = first;
            else
                list[i] = list[i + 1];
    }

    private Line LineFromCollider(List<Vector2> list)
    {
        var line = new Line();
        var points = new List<Point>();
        var segments = new List<Segment>();

        for (var i = 0; i < list.Count; i++)
        {
            var point = new Point();
            point.Position = list[i];
            points.Add(point);
        }

        for (var i = 0; i < list.Count; i++)
        {
            var segment = new Segment();
            segment.A = points[i];
            points[i].LandSegment = segment;
            var bIndex = i + 1;
            if (bIndex >= list.Count) bIndex = 0;
            segment.B = points[bIndex];
            points[bIndex].CircleSegment = segment;
            segments.Add(segment);
        }

        line.Points = points;
        line.Segments = segments;

        return line;
    }

    private int GetNext(int index, int length, bool isCCW)
    {
        var nextIndex = index + (isCCW ? 1 : -1);

        if (nextIndex >= length)
            nextIndex = 0;
        else if (nextIndex < 0) nextIndex = length - 1;

        return nextIndex;
    }
}