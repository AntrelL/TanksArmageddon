using RainyPlace.DI;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Source.Scripts.SOLID
{
    public class PlayerController : ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        private const int LeftDirection = -1;
        private const int RightDirection = 1;
        private PlayerControls _playerControls;

        public event Action ShotActivated;

        public PlayerController(PlayerControls playerControls)
        {
            _playerControls = playerControls;
            _playerControls.ShootButton.onClick.AddListener(OnShootButtonClick);
        }

        public int MovementDirection => GetMovementDirection();

        //как делать игры в Unity?
        //какое архитектурное решение подходит для разработки проекта?
        //нужно прокидывание зависимостей, контроль над очередностью иницализации объектов
        //какая общая архитектура должна быть у проекта?
        //самое лучшее решение
        //????
        
        public void OnEnable()
        {
            _playerControls.ShootButton.onClick.AddListener(OnShootButtonClick);
        }

        public void OnDisable()
        {
            _playerControls.ShootButton.onClick.RemoveListener(OnShootButtonClick);
        }

        private int GetMovementDirection()
        {
            if (_playerControls.MoveLeftButton.IsPressed)
                return LeftDirection;

            if (_playerControls.MoveRightButton.IsPressed)
                return RightDirection;

            return (int)Input.GetAxisRaw(Horizontal);
        }

        private void OnShootButtonClick()
        {
            ShotActivated?.Invoke();
        }
    }
}
