using UnityEditor;
using UnityEngine;

namespace TanksArmageddon
{
    public class ColliderRenderer : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private MeshFilter _meshFilter;

        private void Update()
        {
            if (transform.hasChanged) CreateMesh();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (var p = 0; p < _collider.pathCount; p++)
            for (var i = 0; i < _collider.GetPath(p).Length; i++)
                Handles.Label(_collider.transform.TransformPoint(_collider.GetPath(p)[i]), i.ToString());
        }
#endif
        private void OnValidate()
        {
            CreateMesh();
        }

        public void CreateMesh()
        {
            var mesh = _collider.CreateMesh(true, true);
            _meshFilter.mesh = mesh;
        }
    }
}