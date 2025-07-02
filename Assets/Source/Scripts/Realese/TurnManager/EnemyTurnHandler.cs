using System.Collections;
using UnityEngine;

public class EnemyTurnHandler : MonoBehaviour
{
    [SerializeField] private TurnState _state;

    public IEnumerator ExecuteTurn(EnemyAIController enemy)
    {
        _state.IncrementTurn();
        _state.NotifyTurnStarted(enemy.transform);

        yield return enemy.DoEnemyTurn();

        if (enemy == GetLastActiveEnemy())
        {
            yield return _state.CameraController.TransitionToTarget(_state.Player.transform, 1f);
        }

        _state.NotifyTurnCompleted();
    }

    private EnemyAIController GetLastActiveEnemy()
    {
        for (int i = _state.Enemies.Count - 1; i >= 0; i--)
        {
            if (_state.Enemies[i] != null && _state.Enemies[i].gameObject.activeSelf)
                return _state.Enemies[i];
        }

        return null;
    }
}