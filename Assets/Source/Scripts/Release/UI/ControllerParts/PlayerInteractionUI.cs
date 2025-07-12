using System;
using Source.Scripts.Release.Stuff;
using Source.Scripts.Release.TurnManager;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class PlayerInteractionUI : MonoBehaviour
    {
        [SerializeField] private TurnState _turnState;
        [SerializeField] private Button _playerShootButton;
        [SerializeField] private Button _playerSkipTurnButton;

        private AudioManager _audioManager;

        public event Action SkipTurnButtonPressed;

        public event Action PlayerShootButtonPressed;

        private void OnEnable()
        {
            _turnState.PlayerCanShootChanged += OnPlayerCanShootChanged;
        }

        private void OnDisable()
        {
            _turnState.PlayerCanShootChanged -= OnPlayerCanShootChanged;
        }

        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public void OnShootButtonPressed()
        {
            if (!_playerShootButton.interactable)
                return;

            _playerShootButton.interactable = false;
            _audioManager.PlayButtonClick();
            PlayerShootButtonPressed?.Invoke();
        }

        public void OnSkipTurnButtonPressed()
        {
            if (!_playerSkipTurnButton.interactable)
                return;

            _playerSkipTurnButton.interactable = false;
            _audioManager.PlayButtonClick();
            SkipTurnButtonPressed?.Invoke();
        }

        private void OnPlayerCanShootChanged(bool state)
        {
            _playerShootButton.interactable = state;
            _playerSkipTurnButton.interactable = state;
        }
    }
}
