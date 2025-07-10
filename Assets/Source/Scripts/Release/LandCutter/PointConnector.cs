using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    public class PointConnector
    {
        public void LinkCircular(Line circleLine, bool clockwise)
        {
            for (int i = 0; i < circleLine.Points.Count; i++)
            {
                int nextIndex = GetCircularIndex(i, circleLine.Points.Count, clockwise);
                circleLine.Points[i].NextPoint = circleLine.Points[nextIndex];
            }
        }
        
        public void LinkPointsAcrossLines(
            Line landLine, 
            Line circleLine, 
            PolygonCollider2D circleCollider, 
            int iterationCount)
        {
            List<Point> allPoints = new List<Point>(landLine.Points);
            bool onLand = true;
            Point startPoint = allPoints[0];

            while (allPoints.Count > 0)
            {
                Point thePoint = allPoints[0];

                if (circleCollider.ClosestPoint(thePoint.Position) == thePoint.Position || thePoint.IsCross)
                {
                    allPoints.RemoveAt(0);
                    continue;
                }

                for (int i = 0; i < iterationCount; i++)
                {
                    Line currentLine = onLand ? landLine : circleLine;
                    bool clockwise = onLand;

                    int currentIndex = currentLine.Points.IndexOf(thePoint);
                    int nextIndex = GetCircularIndex(currentIndex, currentLine.Points.Count, clockwise);
                    thePoint.NextPoint = currentLine.Points[nextIndex];

                    allPoints.Remove(thePoint);

                    if (thePoint.NextPoint.IsCross)
                        onLand = !onLand;

                    thePoint = thePoint.NextPoint;

                    if (thePoint == startPoint)
                        break;
                }
            }
        }
        
        private int GetCircularIndex(int index, int length, bool clockwise)
        {
            int next = index + (clockwise ? 1 : -1);
            return (next + length) % length;
        }
    }
}