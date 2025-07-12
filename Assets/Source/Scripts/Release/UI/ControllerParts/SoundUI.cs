using System;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class SoundUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _unmutedSoundCanvasGroup;
        [SerializeField] private CanvasGroup _mutedSoundCanvasGroup;

        private AudioManager _audioManager;

        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public void MuteSound()
        {
            SetSoundState(
                _mutedSoundCanvasGroup,
                _unmutedSoundCanvasGroup,
                _audioManager.StopMainMusic);
        }

        public void UnmuteSound()
        {
            SetSoundState(
                _unmutedSoundCanvasGroup,
                _mutedSoundCanvasGroup,
                _audioManager.PlayMainMusic);
        }

        private void SetSoundState(
            CanvasGroup activeCanvas,
            CanvasGroup inactiveCanvas,
            Action musicAction)
        {
            musicAction?.Invoke();

            activeCanvas.alpha = 1f;
            activeCanvas.interactable = true;
            activeCanvas.blocksRaycasts = true;

            inactiveCanvas.alpha = 0f;
            inactiveCanvas.interactable = false;
            inactiveCanvas.blocksRaycasts = false;
        }
    }
}
