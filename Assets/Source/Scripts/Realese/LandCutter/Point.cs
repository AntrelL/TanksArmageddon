using UnityEngine;

[System.Serializable]
public class Point
{
    [field: SerializeField] public Vector2 Position { get; set; }

    [field: SerializeField] public Point NextPoint { get; set; }

    [field: SerializeField] public bool IsCross { get; set; }

    [field: SerializeField] public Segment LandSegment { get; set; }

    [field: SerializeField] public Segment CircleSegment { get; set; }
}