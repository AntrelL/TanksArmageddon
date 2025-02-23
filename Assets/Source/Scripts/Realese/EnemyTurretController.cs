using Assets.Constructors.FuturisticTanks.Scripts;
using DG.Tweening;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private float _aimDuration = 1f;
    [SerializeField] private Tank _tank;
    [SerializeField] private float _minAngle = -30f;
    [SerializeField] private float _maxAngle = 30f;
    [SerializeField] private DefaultProjectile _projectilePrefab;

    private float _projectileSpeed;
    private TurnManager _turnManager;  // Добавим ссылку на TurnManager

    private void Start()
    {
        DefaultProjectile projectile = _bulletPrefab.GetComponent<DefaultProjectile>();
        if (projectile != null)
        {
            _projectileSpeed = _projectilePrefab.Speed;
        }

        _turnManager = FindObjectOfType<TurnManager>();  // Получаем ссылку на TurnManager
    }

    // Новый метод для расчета случайной цели
    private float GetRandomTargetX(float playerX, float difficultyFactor)
    {
        float offset = playerX * difficultyFactor;
        return Random.Range(playerX - offset, playerX + offset);
    }

    // Новый метод для вычисления угла по баллистической формуле
    private float CalculateBallisticAngle(Vector2 start, Vector2 target, float speed)
    {
        float g = Mathf.Abs(Physics2D.gravity.y);
        float d = Mathf.Abs(target.x - start.x);
        float y = target.y - start.y;

        float v2 = speed * speed;
        float disc = v2 * v2 - g * (g * d * d + 2 * y * v2);
        if (disc < 0) return 0f;

        float sqrtDisc = Mathf.Sqrt(disc);
        float angle = Mathf.Atan((v2 - sqrtDisc) / (g * d));
        return angle * Mathf.Rad2Deg;
    }

    public bool CanShoot(Transform target)
    {
        float difficultyFactor = _turnManager.DifficultyFactor;
        Vector2 firePosition = _firePoint.position;

        float targetX = GetRandomTargetX(target.position.x, difficultyFactor);
        Vector2 targetPos = new Vector2(targetX, target.position.y);

        float angle = CalculateBallisticAngle(firePosition, targetPos, _projectileSpeed);
        return angle >= _minAngle && angle <= _maxAngle;
    }

    public void Shoot(Transform target)
    {
        float difficultyFactor = _turnManager.DifficultyFactor;
        Vector2 firePosition = _firePoint.position;

        float targetX = GetRandomTargetX(target.position.x, difficultyFactor);
        Vector2 targetPos = new Vector2(targetX, target.position.y);

        float angle = CalculateBallisticAngle(firePosition, targetPos, _projectileSpeed);
        angle = Mathf.Clamp(angle, _minAngle, _maxAngle);

        _turret.DORotate(new Vector3(0, 0, angle), _aimDuration)
               .OnComplete(() => Fire(target, targetX, target.position.y));
    }

    private void Fire(Transform target, float targetX, float targetY)
    {
        _tank.Shot();
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

        DefaultProjectile projectile = bullet.GetComponent<DefaultProjectile>();
        if (projectile != null)
        {
            projectile.IsEnemyProjectile = true;
            projectile.SetupBallisticTrajectory(targetX, targetY);
        }

        ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);
    }
}
