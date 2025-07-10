using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.UI
{
    public class MusicStatusUpdater : MonoBehaviour
    {
        [SerializeField] private UIController _uiController;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();

            if (_audioManager.IsMusicOn == true)
            {
                _uiController.UnmuteSound();
            }
            else
            {
                _uiController.MuteSound();
            }
        }
    }
}