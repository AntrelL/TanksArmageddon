using System;
using System.Collections;
using System.Collections.Generic;
using TanksArmageddon;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _blockUICanvas;
    [SerializeField] private Transform _player;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _waitTime = 1.5f;
    [SerializeField] private float _delayBeforeSwitch = 0f;
    [SerializeField] private float _timeSinceSwitch = 0f;

    private bool _introFinished = false;
    private Transform _currentTarget;

    public bool IntroFinished => _introFinished;

    public event Action<bool> UnlockMovement;
    public static event Action ShowTips;

    private void Awake()
    {
        _currentTarget = _player;
    }

    private void OnEnable()
    {
        TurnManager.TurnStarted += OnTurnStarted;
        DefaultProjectile.ProjectileDestroyed += OnProjectileDestroyed;
        EnemyBullet.EnemyBulletDestroyed += OnProjectileDestroyed;
    }

    private void OnDisable()
    {
        TurnManager.TurnStarted -= OnTurnStarted;
        DefaultProjectile.ProjectileDestroyed -= OnProjectileDestroyed;
        EnemyBullet.EnemyBulletDestroyed -= OnProjectileDestroyed;
    }

    private void Start()
    {
        StartCoroutine(CameraIntroSequence());
    }

    private IEnumerator CameraIntroSequence()
    {
        _blockUICanvas.SetActive(true);

        yield return MoveToTarget(_player.position);
        yield return new WaitForSeconds(_waitTime);

        foreach (var enemy in _enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                yield return MoveToTarget(enemy.transform.position);
                yield return new WaitForSeconds(_waitTime);
            }
        }

        yield return MoveToTarget(_player.position);
        yield return new WaitForSeconds(_waitTime);

        _introFinished = true;
        ShowTips?.Invoke();
        UnlockMovement?.Invoke(true);
        _blockUICanvas.SetActive(false);
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        targetPosition.z = transform.position.z;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator TransitionToTarget(Transform newTarget, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(newTarget.position.x, newTarget.position.y, transform.position.z);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        _currentTarget = newTarget;
    }

    private void OnTurnStarted(Transform target)
    {
        _currentTarget = target;
    }

    private void LateUpdate()
    {
        if (_introFinished && _currentTarget != null)
        {
            if (DefaultProjectile.CurrentProjectile != null)
            {
                //убрал .transform у CurrentProjectile
                _currentTarget = DefaultProjectile.CurrentProjectile;
            }

            //добавил этот if
            if (EnemyBullet.CurrentEnemyBullet != null)
            {
                _currentTarget = EnemyBullet.CurrentEnemyBullet;
            }

            if (_timeSinceSwitch >= _delayBeforeSwitch)
            {
                Vector3 targetPosition = new Vector3(_currentTarget.position.x, _currentTarget.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
            }
            else
            {
                _timeSinceSwitch += Time.deltaTime;
            }
        }
    }

    private void OnProjectileDestroyed()
    {
        _timeSinceSwitch = 0f;
        UpdateCameraTarget();
    }

    private void UpdateCameraTarget()
    {
        Transform nextTarget = _player.transform;

        if (TurnManager.CurrentTurnIsPlayer == false)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    nextTarget = enemy.transform;

                    break;
                }
            }
        }
        else
        {
            nextTarget = _player.transform;
        }

        _currentTarget = nextTarget;
    }
}
