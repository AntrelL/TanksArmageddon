using System;
using RainyPlace.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TanksArmageddon
{
    public class PlayerController : MonoBehaviour, ITankController
    {
        private const string Horizontal = nameof(Horizontal);
        
        private const int LeftDirection = -1;
        private const int RightDirection = 1;

        [SerializeField] private PressListener _moveLeftButton;
        [SerializeField] private PressListener _moveRightButton;
        [SerializeField] private Button _shootButton;
        
        public event Action ShotActivated;

        public int MovementDirection => GetMovementDirection();

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
                direction = LeftDirection;

            if (_moveRightButton.IsPressed)
                direction = RightDirection;
            
            return direction;
        }

        private void OnShootButtonClicked() => ShotActivated?.Invoke();
    }
}
