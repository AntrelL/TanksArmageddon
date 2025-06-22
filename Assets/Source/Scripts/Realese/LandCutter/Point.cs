using UnityEngine;

[System.Serializable]
public class Point
{
    public Vector2 Position;
    public Point NextPoint;
    public bool IsCross;
    public Segment LandSegment;
    public Segment CircleSegment;
}