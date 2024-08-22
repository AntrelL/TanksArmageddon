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
         // Получаем позицию мыши в мировых координатах
         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mousePosition.z = 0; // Убираем компонент z, так как у нас 2D игра

         // Вычисляем направление от пушки к мыши
         Vector3 direction = mousePosition - _turret.position;

         // Вычисляем угол в градусах
         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

         // Ограничиваем угол в пределах от -20 до 20 градусов относительно начального угла
         float clampedAngle = Mathf.Clamp(angle - _initialAngle, -20f, 20f) + _initialAngle;

         // Анимируем изменение угла с помощью DOTween
         _turret.DORotate(new Vector3(0, 0, clampedAngle), _rotationSpeed).SetEase(Ease.OutCubic);
    }
}
