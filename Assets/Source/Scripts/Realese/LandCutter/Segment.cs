using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Realese.LandCutter
{
    public class Segment
    {
        [field: SerializeField] public Point A { get; set; }

        [field: SerializeField] public Point B { get; set; }

        [field: SerializeField] public List<Point> CrossPoints { get; set; } = new List<Point>();
    }
}