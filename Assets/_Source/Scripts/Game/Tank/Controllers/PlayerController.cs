using System;
using RainyPlace.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TanksArmageddon
{
    public class PlayerController : MonoBehaviour, ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        private const int PositiveDirection = 1;
        private const int NegativeDirection = -1;

        [SerializeField] private PressListener _moveLeftButton;
        [SerializeField] private PressListener _moveRightButton;
        [SerializeField] private PressListener _rotateUpButton;
        [SerializeField] private PressListener _rotateDownButton;
        [SerializeField] private Button _shootButton;
        
        public event Action ShotActivated;

        public int MovementDirection => GetMovementDirection();
        public int CannonRotateDirection => GetCannonRotateDirection();

        private void OnEnable()
        {
            _shootButton.onClick.AddListener(OnShootButtonClicked);
        }

        private void OnDisable()
        {
            _shootButton.onClick.RemoveListener(OnShootButtonClicked);
        }

        private int GetMovementDirection()
        {
            int direction = (int)Input.GetAxisRaw(Horizontal);

            if (_moveLeftButton.IsPressed)
                direction = NegativeDirection;

            if (_moveRightButton.IsPressed)
                direction = PositiveDirection;
            
            return direction;
        }

        private int GetCannonRotateDirection()
        {
            int direction = 0;
            
            if (_rotateUpButton.IsPressed)
                direction = PositiveDirection;

            if (_rotateDownButton.IsPressed)
                direction = NegativeDirection;

            return direction;
        }

        private void OnShootButtonClicked() => ShotActivated?.Invoke();
    }
}
