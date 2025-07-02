using System.Collections;
using UnityEngine;

public class TurnCycleController : MonoBehaviour
{
    [SerializeField] private PlayerTurnHandler _playerTurn;
    [SerializeField] private EnemyTurnHandler _enemyTurn;
    [SerializeField] private TurnState _turnState;

    private Coroutine _cycle;

    private void Start()
    {
        if (_turnState.CameraController.IntroFinished)
            _cycle = StartCoroutine(TurnCycle());
        else
            _turnState.CameraController.UnlockMovement += OnCameraIntroFinished;
    }

    private IEnumerator TurnCycle()
    {
        while (!_turnState.IsAllEnemiesDead)
        {
            if (_turnState.Player != null && _turnState.Player.gameObject.activeSelf)
            {
                yield return _playerTurn.ExecuteTurn();
                if (_turnState.CheckAllEnemiesDead()) break;
            }

            foreach (var enemy in _turnState.Enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    yield return _enemyTurn.ExecuteTurn(enemy);
                    if (_turnState.CheckAllEnemiesDead()) break;
                }
            }
        }
    }

    private void OnCameraIntroFinished(bool unlocked)
    {
        if (unlocked)
        {
            _turnState.CameraController.UnlockMovement -= OnCameraIntroFinished;
            _cycle = StartCoroutine(TurnCycle());
        }
    }
}