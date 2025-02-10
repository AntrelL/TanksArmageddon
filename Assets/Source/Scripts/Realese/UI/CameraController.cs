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

    private bool followingPlayer = false;
    private bool _introFinished = false;

    public bool IntroFinished => _introFinished;

    public event Action<bool> UnlockMovement;

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

        followingPlayer = true;
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

    private void LateUpdate()
    {
        if (followingPlayer && _player != null)
        {
            Vector3 targetPosition = new Vector3(_player.position.x, _player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }
    }
}
