using System.Collections.Generic;
using UnityEngine;

namespace TanksArmageddon
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private ColliderRenderer _colliderRenderer;

        public void SetPath(List<List<Point>> paths)
        {
            _collider.pathCount = paths.Count;

            for (var i = 0; i < paths.Count; i++)
            {
                var path = new List<Vector2>();

                for (var p = 0; p < paths[i].Count; p++)
                {
                    path.Add(paths[i][p].Position);
                }

                _collider.SetPath(i, path);
            }

            _colliderRenderer.CreateMesh();
        }
    }
}