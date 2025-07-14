using Source.Scripts.Release.Stuff;
using Source.Scripts.Release.UI.ControllerParts;
using UnityEngine;

namespace Source.Scripts.Release.UI
{
    public class MusicStatusUpdater : MonoBehaviour
    {
        [SerializeField] private SoundUI _soundUI;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.Instance;

            if (_audioManager.IsMusicOn)
            {
                _soundUI.UnmuteSound();
            }
            else
            {
                _soundUI.MuteSound();
            }
        }
    }
}