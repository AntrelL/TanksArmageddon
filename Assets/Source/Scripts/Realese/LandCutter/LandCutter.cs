using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Scripts.Realese.LandCutter
{
    public class LandCutter : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _landCollider;
        [SerializeField] private PolygonCollider2D _circleCollider;
        [SerializeField] private int _testIterations = 10;

        public void DoCut()
        {
            List<Vector2> circlePointsPositions = _circleCollider.GetPath(0).ToList();

            for (int i = 0; i < circlePointsPositions.Count; i++)
            {
                circlePointsPositions[i] = _circleCollider.transform.TransformPoint(circlePointsPositions[i]);
            }

            Line circleLine = LineFromCollider(circlePointsPositions);

            List<List<Point>> allSplines = new List<List<Point>>();

            for (int p = 0; p < _landCollider.pathCount; p++)
            {
                List<Vector2> linePointsPositions = _landCollider.GetPath(p).ToList();

                for (int i = 0; i < linePointsPositions.Count; i++)
                {
                    linePointsPositions[i] = _landCollider.transform.TransformPoint(linePointsPositions[i]);
                }

                Line landLine = LineFromCollider(linePointsPositions);

                for (int i = 0; i < landLine.Points.Count; i++)
                {
                    if (_circleCollider.ClosestPoint(landLine.Points[0].Position) == landLine.Points[0].Position)
                    {
                        ReorderList(landLine.Points);
                        ReorderList(landLine.Segments);
                    }
                    else
                    {
                        break;
                    }
                }

                var result = Substraction(landLine, circleLine);
                allSplines.InsertRange(0, result);
            }

            _landCollider.GetComponent<Land>().SetPath(allSplines);
        }

        private List<List<Point>> Substraction(Line landLine, Line circleLine)
        {
            for (int i = 0; i < circleLine.Points.Count; i++)
            {
                int nextIndex = GetNext(i, circleLine.Points.Count, false);
                circleLine.Points[i].NextPoint = circleLine.Points[nextIndex];
            }

            for (int l = 0; l < landLine.Segments.Count; l++)
            {
                Segment landSegment = landLine.Segments[l];
                Vector2 al = landSegment.A.Position;
                Vector2 bl = landSegment.B.Position;

                for (int c = 0; c < circleLine.Segments.Count; c++)
                {
                    Segment circleSegment = circleLine.Segments[c];
                    Vector2 ac = circleLine.Segments[c].A.Position;
                    Vector2 bc = circleLine.Segments[c].B.Position;

                    if (Intersection.IsIntersecting(al, bl, ac, bc))
                    {
                        Vector2 position = Intersection.GetIntersection(al, bl, ac, bc);
                        Point crossPoint = new Point();
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
                List<Point> allPoints = new List<Point>(landLine.Points);
                bool onLand = true;
                Point startPoint = allPoints[0];

                while (allPoints.Count > 0)
                {
                    Point thePoint = allPoints[0];

                    if (_circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                    {
                        allPoints.RemoveAt(0);
                        continue;
                    }

                    for (int i = 0; i < _testIterations; i++)
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

                        int currentIndex = currentLine.Points.IndexOf(thePoint);
                        int nextIndex = GetNext(currentIndex, currentLine.Points.Count, ccw);
                        thePoint.NextPoint = currentLine.Points[nextIndex];
                        allPoints.Remove(thePoint);

                        if (thePoint.NextPoint.IsCross)
                        {
                            onLand = !onLand;
                        }

                        thePoint = thePoint.NextPoint;

                        if (startPoint == thePoint)
                            break;
                    }
                }
            }

            {
                List<List<Point>> allSplines = new List<List<Point>>();
                List<Point> allPoints = new List<Point>(landLine.Points);

                while (allPoints.Count > 0)
                {
                    Point thePoint = allPoints[0];

                    if (_circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                    {
                        allPoints.RemoveAt(0);
                        continue;
                    }
                    else
                    {
                        List<Point> newSpline = new List<Point>();
                        allSplines.Add(newSpline);

                        Point startPoint = thePoint;
                        Point point = thePoint;

                        newSpline.Add(point);
                        allPoints.Remove(point);

                        for (int i = 0; i < _testIterations; i++)
                        {
                            point = point.NextPoint;
                            if (point == startPoint)
                                break;

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
            List<Point> newPoints = new List<Point>();

            for (int s = 0; s < line.Segments.Count; s++)
            {
                Segment segment = line.Segments[s];
                newPoints.Add(segment.A);

                if (segment.CrossPoints.Count > 0)
                {
                    segment.CrossPoints.Sort((p1, p2) =>
                        Vector3.Distance(segment.A.Position, p1.Position).
                            CompareTo(Vector3.Distance(segment.A.Position, p2.Position)));
                }

                newPoints.AddRange(segment.CrossPoints);
            }

            line.Points = newPoints;
        }

        private void ReorderList<T>(List<T> list)
        {
            var first = list[0];

            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    list[i] = first;
                }
                else
                {
                    list[i] = list[i + 1];
                }
            }
        }

        private Line LineFromCollider(List<Vector2> list)
        {
            Line line = new Line();
            List<Point> points = new List<Point>();
            List<Segment> segments = new List<Segment>();

            for (int i = 0; i < list.Count; i++)
            {
                Point point = new Point();
                point.Position = list[i];
                points.Add(point);
            }

            for (int i = 0; i < list.Count; i++)
            {
                Segment segment = new Segment();
                segment.A = points[i];
                points[i].LandSegment = segment;
                int bIndex = i + 1;

                if (bIndex >= list.Count)
                    bIndex = 0;

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
            int nextIndex = index + (isCCW ? 1 : -1);

            if (nextIndex >= length)
            {
                nextIndex = 0;
            }
            else if (nextIndex < 0)
            {
                nextIndex = length - 1;
            }

            return nextIndex;
        }
    }
}
