using System;
using System.Collections;
using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileShooter2D : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float _initialSpeed = 15f;
    [SerializeField] private Transform _player;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Tank _enemyTank;
    [SerializeField] private TurnManager _turnManager;

    [Header("Поворот пушки")] [SerializeField]
    private Transform _turret;

    [SerializeField] private float _rotateDuration = 0.6f;

    [Header("Ограничение угла (±) выстрела")] [SerializeField]
    private float _maxAngleDeviation;

    private float _turretInitialAngle;

    private void Start()
    {
        Debug.Log($"Начальный угол пушки: {_turretInitialAngle}° (rotation z = {_turret.eulerAngles.z})");

        if (_turret != null) _turretInitialAngle = _turret.localEulerAngles.z;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_turret == null || _shootPoint == null)
            return;

        var angleDeg = _turret.eulerAngles.z + 180f;
        var localAngle = Mathf.DeltaAngle(180f, angleDeg);

        Gizmos.color = Mathf.Abs(localAngle) > _maxAngleDeviation ? Color.red : Color.green;

        var angleRad = angleDeg * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Gizmos.DrawRay(_shootPoint.position, direction.normalized * 2f);

        Handles.color = Color.cyan;
        Handles.DrawWireArc(
            _shootPoint.position,
            Vector3.forward,
            Quaternion.Euler(0, 0, 180f - _maxAngleDeviation) * Vector3.right,
            2f * _maxAngleDeviation,
            2f
        );
    }
#endif
    public static event Action EnemyShooted;

    public bool ShootIfPossible()
    {
        var difficultyFactor = _turnManager.DifficultyFactor;

        Vector2 playerPos = _player.position;
        var deviation = Mathf.Abs(playerPos.x) * difficultyFactor;
        var randomX = Random.Range(playerPos.x - deviation, playerPos.x + deviation);
        var target = new Vector2(randomX, playerPos.y);

        if (TryCalculateBallisticAngle2D(target, out var lowAngleDeg, out var highAngleDeg))
        {
            var chosenAngle = lowAngleDeg;

            Vector2 toTarget = _player.position - _shootPoint.position;
            var isTargetLeft = toTarget.x < 0f;

            if (!isTargetLeft)
            {
                Debug.Log("Цель справа, стреляем только влево.");
                return false;
            }

            var usedAngle = -chosenAngle;
            var turretTargetAngle = usedAngle;

            var angleOffset = Mathf.DeltaAngle(0f, turretTargetAngle);

            if (Mathf.Abs(angleOffset) > _maxAngleDeviation)
            {
                Debug.Log(
                    $"Выстрел невозможен: угол отклонения {angleOffset}° выходит за пределы ±{_maxAngleDeviation}°");

                return false;
            }

            StartCoroutine(RotateThenShoot(turretTargetAngle));

            return true;
        }

        Debug.Log("Выстрел невозможен: нет баллистического решения.");

        return false;
    }

    private IEnumerator RotateThenShoot(float targetAngle)
    {
        var startAngle = _turret.eulerAngles.z;
        var elapsedTime = 0f;

        while (elapsedTime < _rotateDuration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / _rotateDuration);

            var interpolatedAngle = Mathf.LerpAngle(startAngle, targetAngle, t);
            _turret.eulerAngles = new Vector3(0f, 0f, interpolatedAngle);

            yield return null;
        }

        _turret.eulerAngles = new Vector3(0f, 0f, targetAngle);

        EnemyShooted?.Invoke();
        _enemyTank.Shot();
        ShootBullet();
    }

    private void ShootBullet()
    {
        if (_bulletPrefab == null)
            return;

        var bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        var rigidbody = bullet.GetComponent<Rigidbody2D>();

        var turretAngleDeg = _turret.eulerAngles.z;

        var angleRad = turretAngleDeg * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * -1;

        rigidbody.velocity = direction * _initialSpeed;

        var flash = Instantiate(_muzzleFlash, _muzzlePoint.position, _muzzlePoint.rotation);
        flash.Play();

        Destroy(flash.gameObject, flash.main.duration);
    }

    private bool TryCalculateBallisticAngle2D(Vector2 targetPos,
        out float lowAngleDeg,
        out float highAngleDeg)
    {
        lowAngleDeg = 0f;
        highAngleDeg = 0f;

        var toTarget = targetPos - (Vector2) _shootPoint.position;

        var xDistance = toTarget.x;
        var xAbs = Mathf.Abs(xDistance);
        var yOffset = toTarget.y;

        var g = -Physics2D.gravity.y;
        var v0 = _initialSpeed;
        var v2 = v0 * v0;
        var v4 = v2 * v2;

        if (xAbs < 0.01f)
            return false;

        var discriminant = v4 - g * (g * xAbs * xAbs + 2f * yOffset * v2);

        if (discriminant < 0f)
            return false;

        var sqrtDisc = Mathf.Sqrt(discriminant);

        var angleRad1 = Mathf.Atan((v2 + sqrtDisc) / (g * xAbs));
        var angleRad2 = Mathf.Atan((v2 - sqrtDisc) / (g * xAbs));

        var angle1Deg = angleRad1 * Mathf.Rad2Deg;
        var angle2Deg = angleRad2 * Mathf.Rad2Deg;

        lowAngleDeg = Mathf.Min(angle1Deg, angle2Deg);
        highAngleDeg = Mathf.Max(angle1Deg, angle2Deg);

        return true;
    }
}