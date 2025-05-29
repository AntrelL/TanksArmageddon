using RainyPlace.DI;
using RainyPlace.UI;
using System;
using UnityEngine;

namespace RainyPlace.Core
{
    public class PlayerController : Script, ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        private const int LeftDirection = -1;
        private const int RightDirection = 1;

        private PlayerControls _playerControls;
        private Event _shotActivated = new();

        public PlayerController(PlayerControls playerControls)
        {
            _playerControls = playerControls;

            IProtectedEvent<Action> shootButtonClickEvent = 
                EventConverter.GetProtectedEvent(_playerControls.ShootButton.onClick);

            Link(shootButtonClickEvent, _shotActivated.Invoke);
        }

        public IProtectedEvent<Action> ShotActivated => _shotActivated;

        public int MovementDirection => GetMovementDirection();

        private int GetMovementDirection()
        {
            if (_playerControls.MoveLeftButton.IsPressed)
                return LeftDirection;

            if (_playerControls.MoveRightButton.IsPressed)
                return RightDirection;

            return (int)Input.GetAxisRaw(Horizontal);
        }
    }
}
