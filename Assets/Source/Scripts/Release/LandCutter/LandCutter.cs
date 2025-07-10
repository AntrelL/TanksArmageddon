using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Release.LandCutter
{
    public class LandCutter : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _landCollider;
        [SerializeField] private PolygonCollider2D _circleCollider;
        [SerializeField] private int _testIterations = 10;

        private readonly LineFactory _lineFactory = new LineFactory();
        private readonly PointConnector _pointConnector = new PointConnector();
        private readonly CuttingToolkit _cuttingToolkit = new CuttingToolkit();

        public void DoCut()
        {
            Line circleLine = _lineFactory.CreateFromCollider(_circleCollider, 0);
            var allSplines = new List<List<Point>>();

            for (int p = 0; p < _landCollider.pathCount; p++)
            {
                Line landLine = _lineFactory.CreateFromCollider(_landCollider, p);
                _cuttingToolkit.AlignLineToOutside(landLine, _circleCollider);

                var result = Subtract(landLine, circleLine);
                allSplines.InsertRange(0, result);
            }

            _landCollider.GetComponent<Land>().SetPath(allSplines);
        }

        private List<List<Point>> Subtract(Line landLine, Line circleLine)
        {
            _pointConnector.LinkCircular(circleLine, false);
            _cuttingToolkit.InjectCrossPoints(landLine, circleLine);

            _lineFactory.Recalculate(landLine);
            _lineFactory.Recalculate(circleLine);

            _pointConnector.LinkPointsAcrossLines(landLine, circleLine, _circleCollider, _testIterations);
            return _cuttingToolkit.CollectSplines(landLine, _circleCollider, _testIterations);
        }
    }
}