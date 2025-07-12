using System;
using System.Collections;
using Source.Scripts.Release.Projectiles;
using Source.Scripts.Release.UI.ControllerParts;
using UnityEngine;

namespace Source.Scripts.Release.TurnManager
{
    public class PlayerTurnHandler : MonoBehaviour
    {
        [SerializeField] private TurnState _state;

        public IEnumerator ExecuteTurn()
        {
            _state.IncrementTurn();
            _state.SetPlayerTurn(true);
            _state.NotifyTurnStarted(_state.Player.transform);
            _state.SetPlayerControl(true);

            bool shot = false, skip = false;

            Action onShot = () => shot = true;
            Action onSkip = () => skip = true;

            PlayerInteractionUI playerInteractionUI = _state.UI.PlayerInteractionUI;

            playerInteractionUI.PlayerShootButtonPressed += onShot;
            playerInteractionUI.SkipTurnButtonPressed += onSkip;

            yield return new WaitUntil(() => shot || skip);

            _state.SetPlayerControl(false);
            playerInteractionUI.PlayerShootButtonPressed -= onShot;
            playerInteractionUI.SkipTurnButtonPressed -= onSkip;

            if (!skip)
            {
                bool projectileEnded = false;
                void OnProjectileDestroyed() => projectileEnded = true;

                ProjectileTracker.Instance.ProjectileDestroyed += OnProjectileDestroyed;

                yield return new WaitUntil(() => projectileEnded);

                ProjectileTracker.Instance.ProjectileDestroyed -= OnProjectileDestroyed;
            }

            var next = _state.GetNextTarget();
            if (next != null)
                yield return _state.CameraController.TransitionToTarget(next, 1f);

            _state.NotifyTurnCompleted();
            _state.SetPlayerTurn(false);
        }
    }
}