using System;
using System.Collections;
using TanksArmageddon;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _blockUICanvas;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _enemy;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _waitTime = 1.5f;

    private bool _introFinished = false;
    private Transform _currentTarget;

    public bool IntroFinished => _introFinished;

    public event Action<bool> UnlockMovement;

    private void Awake()
    {
        _currentTarget = _player;
    }

    private void OnEnable()
    {
        TurnManager.TurnStarted += OnTurnStarted;
    }

    private void OnDisable()
    {
        TurnManager.TurnStarted -= OnTurnStarted;
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

        yield return MoveToTarget(_enemy.position);
        yield return new WaitForSeconds(_waitTime);

        yield return MoveToTarget(_player.position);
        yield return new WaitForSeconds(_waitTime);

        _introFinished = true;
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
                _currentTarget = DefaultProjectile.CurrentProjectile;
            }

            Vector3 targetPosition = new Vector3(_currentTarget.position.x, _currentTarget.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }
    }
}
