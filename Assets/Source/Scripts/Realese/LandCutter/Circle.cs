using TanksArmageddon;
using UnityEngine;

[ExecuteAlways]
public class Circle : MonoBehaviour
{
    [SerializeField] ColliderRenderer _colliderRenderer;
    [SerializeField] int _sides;
    [SerializeField] PolygonCollider2D _collider;

    private void OnValidate()
    {
        CreateCircle();
    }

    void CreateCircle()
    {
        _collider.CreatePrimitive(_sides, Vector2.one);
        _colliderRenderer.CreateMesh();
    }
}
