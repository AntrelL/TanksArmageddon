using System.Linq;
using UnityEngine;

namespace TanksArmageddon
{
    [RequireComponent(typeof(Collider2D), typeof(MeshFilter))]
    public class Collider2DVisualizer : MonoBehaviour // Temp
    {
        public void Initialize()
        {
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            Mesh mesh = collider.CreateMesh(false, false);
            mesh.vertices = mesh.vertices.Select(transform.InverseTransformPoint).ToArray();

            meshFilter.mesh = mesh;
        }
    }
}
