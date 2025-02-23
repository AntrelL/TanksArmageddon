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
    [SerializeField] private float _maxAngleDeviation = 30f;

    //private float _turretInitialAngle;
    public static event Action EnemyShooted;


    private void Start()
    {
        /*if (_turret != null)
        {
            _turretInitialAngle = _turret.eulerAngles.z;
        }*/
    }

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
            Debug.Log("Угол стрельбы: " + usedAngle);

            //float angleDifference = userAngle - _turretInitialAngle;

            /*if (Mathf.Abs(angleDifference) > _maxAngleDeviation)
            {
                Debug.Log($"Выстрел невозможен: угол {userAngle}° за пределами ±{_maxAngleDeviation}°");
            /
                return;
            }*/

            StartCoroutine(RotateThenShoot(usedAngle));

            return true;
        }
        else
        {
            Debug.Log("Выстрел невозможен: нет баллистического решения.");

            return false;
        }
    }

    private IEnumerator RotateThenShoot(float targetUserAngle)
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
    }

    private void ShootBullet()
    {
        if (_bulletPrefab == null)
            return;

        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        float turretAngleDeg = _turret.eulerAngles.z;

        float angleRad = (turretAngleDeg + 180f) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

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
}
