using System.Collections.Generic;
using Source.Scripts.Release.Utils;
using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    public class CuttingToolkit
    {
        public void InjectCrossPoints(Line landLine, Line circleLine)
        {
            foreach (Segment landSegment in landLine.Segments)
            {
                Vector2 al = landSegment.A.Position;
                Vector2 bl = landSegment.B.Position;

                foreach (Segment circleSegment in circleLine.Segments)
                {
                    Vector2 ac = circleSegment.A.Position;
                    Vector2 bc = circleSegment.B.Position;

                    if (Intersection.IsIntersecting(al, bl, ac, bc))
                    {
                        Vector2 position = Intersection.GetIntersection(al, bl, ac, bc);

                        Point crossPoint = new Point
                        {
                            Position = position,
                            LandSegment = landSegment,
                            CircleSegment = circleSegment,
                            IsCross = true
                        };

                        landSegment.CrossPoints.Add(crossPoint);
                        circleSegment.CrossPoints.Add(crossPoint);
                    }
                }
            }
        }

        public void AlignLineToOutside(Line landLine, PolygonCollider2D circleCollider)
        {
            for (int i = 0; i < landLine.Points.Count; i++)
            {
                if (circleCollider.ClosestPoint(landLine.Points[0].Position) == landLine.Points[0].Position)
                {
                    landLine.Points.RotateLeft();
                    landLine.Segments.RotateLeft();
                }
                else
                {
                    break;
                }
            }
        }

        public List<List<Point>> CollectSplines(Line landLine, PolygonCollider2D circleCollider, int iterations)
        {
            var allSplines = new List<List<Point>>();
            var allPoints = new List<Point>(landLine.Points);

            while (allPoints.Count > 0)
            {
                Point thePoint = allPoints[0];

                if (circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                {
                    allPoints.RemoveAt(0);
                    continue;
                }

                var newSpline = new List<Point>() { thePoint };
                allSplines.Add(newSpline);
                Point startPoint = thePoint;
                allPoints.Remove(thePoint);

                for (int i = 0; i < iterations; i++)
                {
                    thePoint = thePoint.NextPoint;
                    if (thePoint == startPoint)
                        break;

                    newSpline.Add(thePoint);
                    allPoints.Remove(thePoint);
                }
            }

            return allSplines;
        }
    }
}