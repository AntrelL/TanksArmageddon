using Source.Scripts.Realese.Stuff;
using Source.Scripts.Realese.TurnManager;
using Source.Scripts.Realese.UI;
using UnityEngine;

namespace Source.Scripts.Realese.Player
{
    public class PlayerEvents: MonoBehaviour
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private TurnState _turnState;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private EdgeOfMap _edgeOfMap;
        [SerializeField] private PlayerMovement _movement;

        private void OnEnable()
        {
            _cameraController.UnlockMovement += OnControl;
            _turnState.CanPlayerControl += OnControl;
            _edgeOfMap.CollisionWithPlayer += _health.TakeDamage;
        }

        private void OnDisable()
        {
            _cameraController.UnlockMovement -= OnControl;
            _turnState.CanPlayerControl -= OnControl;
            _edgeOfMap.CollisionWithPlayer -= _health.TakeDamage;
        }

        private void OnControl(bool canMove)
        {
            _movement.SetCanMove(canMove);
        }
    }
}