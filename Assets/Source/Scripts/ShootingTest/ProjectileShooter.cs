using UnityEngine;
using System.Collections;
using Assets.Constructors.FuturisticTanks.Scripts;
using System;

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

    [Header("Поворот пушки")]
    [SerializeField] private Transform _turret;
    [SerializeField] private float _rotateDuration = 0.6f;

    [Header("Ограничение угла (±) выстрела")]
    [SerializeField] private float _maxAngleDeviation;

    private float _turretInitialAngle;
    public static event Action EnemyShooted;


    private void Start()
    {
        Debug.Log($"Начальный угол пушки: {_turretInitialAngle}° (rotation z = {_turret.eulerAngles.z})");

        if (_turret != null)
        {
            _turretInitialAngle = _turret.localEulerAngles.z;
        }
    }

    /*public bool ShootIfPossible()
    {
        float difficultyFactor = _turnManager.DifficultyFactor;

        Vector2 playerPos = _player.position;
        float deviation = Mathf.Abs(playerPos.x) * difficultyFactor;
        float randomX = UnityEngine.Random.Range(playerPos.x - deviation, playerPos.x + deviation);
        Vector2 target = new Vector2(randomX, playerPos.y);

        if (TryCalculateBallisticAngle2D(target, out float lowAngleDeg, out float highAngleDeg))
        {
            float chosenAngle = lowAngleDeg;

            Vector2 toTarget = _player.position - _shootPoint.position;
            bool isTargetLeft = (toTarget.x < 0f);

            if (!isTargetLeft)
            {
                Debug.Log("Цель справа, стреляем только влево.");

                return false;
            }

            float usedAngle = -chosenAngle;

            if (Mathf.Abs(usedAngle + 5f) > _maxAngleDeviation)
            {
                Debug.Log($"Выстрел невозможен: требуется угол {usedAngle}° за пределами допустимого" +
                    $" диапазона ±{_maxAngleDeviation}° относительно стартового угла {_turretInitialAngle}°");

                return false;
            }

            StartCoroutine(RotateThenShoot(usedAngle));

            return true;
        }
        else
        {
            Debug.Log("Выстрел невозможен: нет баллистического решения.");

            return false;
        }
    }*/

    public bool ShootIfPossible()
    {
        float difficultyFactor = _turnManager.DifficultyFactor;

        Vector2 playerPos = _player.position;
        float deviation = Mathf.Abs(playerPos.x) * difficultyFactor;
        float randomX = UnityEngine.Random.Range(playerPos.x - deviation, playerPos.x + deviation);
        Vector2 target = new Vector2(randomX, playerPos.y);

        if (TryCalculateBallisticAngle2D(target, out float lowAngleDeg, out float highAngleDeg))
        {
            float chosenAngle = lowAngleDeg;

            Vector2 toTarget = _player.position - _shootPoint.position;
            bool isTargetLeft = (toTarget.x < 0f);

            if (!isTargetLeft)
            {
                Debug.Log("Цель справа, стреляем только влево.");
                return false;
            }

            float usedAngle = -chosenAngle;
            //float turretTargetAngle = 180f + usedAngle;
            float turretTargetAngle = usedAngle;

            //float angleOffset = Mathf.DeltaAngle(180f, turretTargetAngle);
            float angleOffset = Mathf.DeltaAngle(0f, turretTargetAngle);

            if (Mathf.Abs(angleOffset) > _maxAngleDeviation)
            {
                Debug.Log($"Выстрел невозможен: угол отклонения {angleOffset}° выходит за пределы ±{_maxAngleDeviation}°");
                return false;
            }

            StartCoroutine(RotateThenShoot(turretTargetAngle));
            return true;
        }
        else
        {
            Debug.Log("Выстрел невозможен: нет баллистического решения.");
            return false;
        }
    }

    private IEnumerator RotateThenShoot(float targetAngle)
    {
        float startAngle = _turret.eulerAngles.z;
        float elapsedTime = 0f;

        while (elapsedTime < _rotateDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _rotateDuration);

            float interpolatedAngle = Mathf.LerpAngle(startAngle, targetAngle, t);
            _turret.eulerAngles = new Vector3(0f, 0f, interpolatedAngle);

            yield return null;
        }

        _turret.eulerAngles = new Vector3(0f, 0f, targetAngle);

        EnemyShooted?.Invoke();
        _enemyTank.Shot();
        ShootBullet();
    }

    /*private IEnumerator RotateThenShoot(float targetUserAngle)
    {
        float startAngle = _turret.eulerAngles.z;
        float currentUserAngle = startAngle;
        float elapsedTime = 0f;

        while (elapsedTime < _rotateDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _rotateDuration);

            float newAngle = Mathf.LerpAngle(currentUserAngle, targetUserAngle, t);
            _turret.eulerAngles = new Vector3(0f, 0f, newAngle);

            yield return null;
        }

        _turret.eulerAngles = new Vector3(0f, 0f, targetUserAngle);
        EnemyShooted?.Invoke();
        _enemyTank.Shot();
        ShootBullet();
    }*/

    private void ShootBullet()
    {
        if (_bulletPrefab == null)
            return;

        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        float turretAngleDeg = _turret.eulerAngles.z;

        //float angleRad = (turretAngleDeg + 180f) * Mathf.Deg2Rad;
        float angleRad = turretAngleDeg * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * -1;

        rigidbody.velocity = direction * _initialSpeed;

        ParticleSystem flash = Instantiate(_muzzleFlash, _muzzlePoint.position, _muzzlePoint.rotation);
        flash.Play();

        Destroy(flash.gameObject, flash.main.duration);
    }

    private bool TryCalculateBallisticAngle2D(Vector2 targetPos,
        out float lowAngleDeg,
        out float highAngleDeg)
    {
        lowAngleDeg = 0f;
        highAngleDeg = 0f;

        Vector2 toTarget = targetPos - (Vector2)_shootPoint.position;

        float xDistance = toTarget.x;
        float xAbs = Mathf.Abs(xDistance);
        float yOffset = toTarget.y;

        float g = -Physics2D.gravity.y;
        float v0 = _initialSpeed;
        float v2 = v0 * v0;
        float v4 = v2 * v2;

        if (xAbs < 0.01f)
            return false;

        float discriminant = v4 - g * (g * xAbs * xAbs + 2f * yOffset * v2);
        if (discriminant < 0f)
            return false;

        float sqrtDisc = Mathf.Sqrt(discriminant);

        float angleRad1 = Mathf.Atan((v2 + sqrtDisc) / (g * xAbs));
        float angleRad2 = Mathf.Atan((v2 - sqrtDisc) / (g * xAbs));

        float angle1Deg = angleRad1 * Mathf.Rad2Deg;
        float angle2Deg = angleRad2 * Mathf.Rad2Deg;

        lowAngleDeg = Mathf.Min(angle1Deg, angle2Deg);
        highAngleDeg = Mathf.Max(angle1Deg, angle2Deg);

        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_turret == null || _shootPoint == null)
            return;

        //float angleDeg = _turret.eulerAngles.z + 180f;
        //float localAngle = Mathf.DeltaAngle(180f, angleDeg); // отклонение от "влево" (180°)
        float angleDeg = _turret.eulerAngles.z + 180f;
        float localAngle = Mathf.DeltaAngle(180f, angleDeg);

        // Определяем цвет стрелки: зелёный — ок, красный — вне диапазона
        Gizmos.color = Mathf.Abs(localAngle) > _maxAngleDeviation ? Color.red : Color.green;

        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Gizmos.DrawRay(_shootPoint.position, direction.normalized * 2f);

        // Рисуем допустимые дуги (± MaxAngleDeviation)
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireArc(
            _shootPoint.position,
            Vector3.forward,
            Quaternion.Euler(0, 0, 180f - _maxAngleDeviation) * Vector3.right,
            2f * _maxAngleDeviation,
            2f
        );
        /*UnityEditor.Handles.DrawWireArc(
    _shootPoint.position,
    Vector3.forward,
    Quaternion.Euler(0, 0, -_maxAngleDeviation) * Vector3.right,
    2f * _maxAngleDeviation,
    2f
        );*/
    }
#endif

}
