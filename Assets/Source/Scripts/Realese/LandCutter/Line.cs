using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line
{
    [field: SerializeField] public List<Point> Points { get; set; }
    
    [field: SerializeField] public List<Segment> Segments { get; set; }
}