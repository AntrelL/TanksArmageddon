using TanksArmageddon.CompositeRoot;
using UnityEngine;

namespace TanksArmageddon.Landscape
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(MeshFilter))]
    public class ColliderVisualizer : MonoBehaviour, IConstructible
    {
        public void Construct()
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            meshFilter.mesh = collider2D.CreateMesh(true, true);
        }
    }
}
