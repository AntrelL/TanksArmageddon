using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    [System.Serializable]
    public class Line
    {
        [field: SerializeField] public List<Point> Points { get; set; }

        [field: SerializeField] public List<Segment> Segments { get; set; }
    }
}