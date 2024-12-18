using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class TankTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private float _rotationSpeed = 0.2f;

    private float _initialAngle;

    void Start()
    {
        _initialAngle = _turret.rotation.eulerAngles.z;
    }

    void Update()
    {
        RotateTurretTowardsMouse();
    }

    void RotateTurretTowardsMouse()
    {
         // �������� ������� ���� � ������� �����������
         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mousePosition.z = 0; // ������� ��������� z, ��� ��� � ��� 2D ����

         // ��������� ����������� �� ����� � ����
         Vector3 direction = mousePosition - _turret.position;

         // ��������� ���� � ��������
         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

         // ������������ ���� � �������� �� -20 �� 20 �������� ������������ ���������� ����
         float clampedAngle = Mathf.Clamp(angle - _initialAngle, -20f, 20f) + _initialAngle;

         // ��������� ��������� ���� � ������� DOTween
         _turret.DORotate(new Vector3(0, 0, clampedAngle), _rotationSpeed).SetEase(Ease.OutCubic);
    }
}
