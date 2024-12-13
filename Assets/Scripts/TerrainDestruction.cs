using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDestruction : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _terrainCollider;  // PolygonCollider2D ландшафта
    [SerializeField] private  MeshRenderer _terrainRenderer;        // MeshRenderer для визуализации
    [SerializeField] private MeshFilter _meshFilter;               // MeshFilter для работы с Mesh

    private Mesh _terrainMesh;                   // Меш ландшафта
    private Vector3[] _vertices;                // Вершины меша

    private void Start()
    {
        // Проверяем, есть ли PolygonCollider2D на объекте
        if (_terrainRenderer == null || _terrainCollider == null || _meshFilter == null)
        {
            Debug.LogError("Не установлены компоненты MeshRenderer, MeshFilter или PolygonCollider2D.");
            return;
        }

        // Получаем Mesh из MeshFilter
        _terrainMesh = _meshFilter.mesh;
        _vertices = _terrainMesh.vertices;
    }

    // Вызывается при попадании снаряда
    public void DestroyTerrain(Vector2 worldPosition, float radius)
    {
        if (_terrainMesh == null) return;

        // Преобразуем мировые координаты в локальные координаты объекта
        Vector2 localPosition = transform.InverseTransformPoint(worldPosition);

        // Обновляем вершины, чтобы удалить разрушенные области
        for (int i = 0; i < _vertices.Length; i++)
        {
            Vector2 vertex2D = new Vector2(_vertices[i].x, _vertices[i].y);

            if (Vector2.Distance(vertex2D, localPosition) <= radius)
            {
                // Устанавливаем вершины в точку разрушения, чтобы "стереть" часть
                _vertices[i] = new Vector3(_vertices[i].x, _vertices[i].y, 0); // Ставим их в 2D-плоскость
            }
        }

        // Обновляем меш с новыми вершинами
        _terrainMesh.vertices = _vertices;
        _terrainMesh.RecalculateBounds();   // Пересчитываем границы меша
        _terrainMesh.RecalculateNormals();  // Пересчитываем нормали

        // Обновляем коллайдер
        UpdateCollider();

        // Применяем изменения в MeshRenderer
        _meshFilter.mesh = _terrainMesh;
    }

    // Обновляем коллайдер с новыми точками после разрушения
    void UpdateCollider()
    {
        // Получаем обновленные точки из Mesh
        Vector2[] updatedPoints = new Vector2[_terrainCollider.points.Length];

        for (int i = 0; i < _terrainCollider.points.Length; i++)
        {
            updatedPoints[i] = new Vector2(_terrainCollider.points[i].x, _terrainCollider.points[i].y);
        }

        // Создаем новый список для обновленных точек
        System.Collections.Generic.List<Vector2> newColliderPoints = new System.Collections.Generic.List<Vector2>();

        // Добавляем точки, которые не попали в разрушенную область
        for (int i = 0; i < updatedPoints.Length; i++)
        {
            if (_terrainMesh.vertices[i].z == 0)  // Если вершина была разрушена
            {
                newColliderPoints.Add(updatedPoints[i]);
            }
        }

        // Обновляем путь в PolygonCollider2D
        _terrainCollider.SetPath(0, newColliderPoints.ToArray());
    }
}
