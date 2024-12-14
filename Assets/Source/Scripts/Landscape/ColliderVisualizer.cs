using UnityEngine;

public class ColliderVisualizer : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private MeshFilter _meshFilter;

    private void Start()
    {
        Mesh mesh = _collider.CreateMesh(true, true);
        _meshFilter.mesh = mesh;
    }
}
