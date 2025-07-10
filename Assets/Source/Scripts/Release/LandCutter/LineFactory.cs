using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    public class LineFactory
    {
        public Line CreateFromCollider(PolygonCollider2D polygonCollider, int pathIndex)
        {
            List<Vector2> localPoints = polygonCollider.GetPath(pathIndex).ToList();

            for (int i = 0; i < localPoints.Count; i++)
                localPoints[i] = polygonCollider.transform.TransformPoint(localPoints[i]);

            return CreateFromPoints(localPoints);
        }
        
        public void Recalculate(Line line)
        {
            var newPoints = new List<Point>();

            foreach (Segment segment in line.Segments)
            {
                newPoints.Add(segment.A);

                if (segment.CrossPoints.Count > 0)
                {
                    segment.CrossPoints.Sort((p1, p2) =>
                        Vector2.Distance(segment.A.Position, p1.Position)
                            .CompareTo(Vector2.Distance(segment.A.Position, p2.Position)));
                }

                newPoints.AddRange(segment.CrossPoints);
            }

            line.Points = newPoints;
        }
        
        private Line CreateFromPoints(List<Vector2> worldPoints)
        {
            List<Point> points = new List<Point>();
            List<Segment> segments = new List<Segment>();

            for (int i = 0; i < worldPoints.Count; i++)
            {
                var point = new Point { Position = worldPoints[i] };
                points.Add(point);
            }

            for (int i = 0; i < points.Count; i++)
            {
                int next = (i + 1) % points.Count;

                var segment = new Segment
                {
                    A = points[i],
                    B = points[next]
                };

                points[i].LandSegment = segment;
                points[next].CircleSegment = segment;

                segments.Add(segment);
            }

            return new Line
            {
                Points = points,
                Segments = segments
            };
        }
    }
}