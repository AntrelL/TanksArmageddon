using UnityEngine;
using TanksArmageddon.CompositeRoot;

namespace TanksArmageddon.Landscape
{
    public class ColliderVisualizer : MonoScript, IConstructable
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private MeshFilter _meshFilter;

        public void Construct()
        {
            Mesh mesh = _collider.CreateMesh(true, true);
            _meshFilter.mesh = mesh;
        }
    }
}
