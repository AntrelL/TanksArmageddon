using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDestruction : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _terrainCollider;  // PolygonCollider2D ���������
    [SerializeField] private  MeshRenderer _terrainRenderer;        // MeshRenderer ��� ������������
    [SerializeField] private MeshFilter _meshFilter;               // MeshFilter ��� ������ � Mesh

    private Mesh _terrainMesh;                   // ��� ���������
    private Vector3[] _vertices;                // ������� ����

    private void Start()
    {
        // ���������, ���� �� PolygonCollider2D �� �������
        if (_terrainRenderer == null || _terrainCollider == null || _meshFilter == null)
        {
            Debug.LogError("�� ����������� ���������� MeshRenderer, MeshFilter ��� PolygonCollider2D.");
            return;
        }

        // �������� Mesh �� MeshFilter
        _terrainMesh = _meshFilter.mesh;
        _vertices = _terrainMesh.vertices;
    }

    // ���������� ��� ��������� �������
    public void DestroyTerrain(Vector2 worldPosition, float radius)
    {
        if (_terrainMesh == null) return;

        // ����������� ������� ���������� � ��������� ���������� �������
        Vector2 localPosition = transform.InverseTransformPoint(worldPosition);

        // ��������� �������, ����� ������� ����������� �������
        for (int i = 0; i < _vertices.Length; i++)
        {
            Vector2 vertex2D = new Vector2(_vertices[i].x, _vertices[i].y);

            if (Vector2.Distance(vertex2D, localPosition) <= radius)
            {
                // ������������� ������� � ����� ����������, ����� "�������" �����
                _vertices[i] = new Vector3(_vertices[i].x, _vertices[i].y, 0); // ������ �� � 2D-���������
            }
        }

        // ��������� ��� � ������ ���������
        _terrainMesh.vertices = _vertices;
        _terrainMesh.RecalculateBounds();   // ������������� ������� ����
        _terrainMesh.RecalculateNormals();  // ������������� �������

        // ��������� ���������
        UpdateCollider();

        // ��������� ��������� � MeshRenderer
        _meshFilter.mesh = _terrainMesh;
    }

    // ��������� ��������� � ������ ������� ����� ����������
    void UpdateCollider()
    {
        // �������� ����������� ����� �� Mesh
        Vector2[] updatedPoints = new Vector2[_terrainCollider.points.Length];

        for (int i = 0; i < _terrainCollider.points.Length; i++)
        {
            updatedPoints[i] = new Vector2(_terrainCollider.points[i].x, _terrainCollider.points[i].y);
        }

        // ������� ����� ������ ��� ����������� �����
        System.Collections.Generic.List<Vector2> newColliderPoints = new System.Collections.Generic.List<Vector2>();

        // ��������� �����, ������� �� ������ � ����������� �������
        for (int i = 0; i < updatedPoints.Length; i++)
        {
            if (_terrainMesh.vertices[i].z == 0)  // ���� ������� ���� ���������
            {
                newColliderPoints.Add(updatedPoints[i]);
            }
        }

        // ��������� ���� � PolygonCollider2D
        _terrainCollider.SetPath(0, newColliderPoints.ToArray());
    }
}