using UnityEngine;

public class ColliderRenderer : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private MeshFilter _meshFilter;

    private void Start()
    {
        Mesh mesh = _collider.CreateMesh(true, true);
        _meshFilter.mesh = mesh;
    }
}