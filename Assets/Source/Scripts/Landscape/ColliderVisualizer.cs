using UnityEngine;

namespace TanksArmageddon.Landscape
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(MeshFilter))]
    public class ColliderVisualizer : MonoBehaviour
    {
        public void Construct()
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            Mesh mesh = collider2D.CreateMesh(true, true);
            Vector3[] vertices = mesh.vertices; 

            for (int i = 0; i < vertices.Length; i++) 
            { 
                vertices[i] = transform.InverseTransformPoint(vertices[i]); 
            } 

            mesh.vertices = vertices;
            meshFilter.mesh = mesh;
        }
    }
}
