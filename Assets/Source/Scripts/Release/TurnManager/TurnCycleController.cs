using System.Collections;
using UnityEngine;

namespace Source.Scripts.Release.TurnManager
{
    public class TurnCycleController : MonoBehaviour
    {
        [SerializeField] private PlayerTurnHandler _playerTurn;
        [SerializeField] private EnemyTurnHandler _enemyTurn;
        [SerializeField] private TurnState _turnState;

        private void Start()
        {
            if (_turnState.CameraController.IntroFinished)
                StartCoroutine(TurnCycle());
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
                StartCoroutine(TurnCycle());
            }
        }
    }
}
