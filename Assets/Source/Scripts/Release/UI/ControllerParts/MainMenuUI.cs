using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _pauseCanvasGroup;
        [SerializeField] private CanvasGroup _continueCanvasGroup;
        [SerializeField] private CanvasGroup _mainMenuCanvasGroup;

        private AudioManager _audioManager;

        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public void OpenMainMenu()
        {
            _audioManager.PlayButtonClick();
            Time.timeScale = 0f;

            SetCanvasState(_mainMenuCanvasGroup, true);
            SetCanvasState(_pauseCanvasGroup, false);
            SetCanvasState(_continueCanvasGroup, true);
        }

        public void CloseMainMenu()
        {
            _audioManager.PlayButtonClick();
            Time.timeScale = 1f;

            SetCanvasState(_pauseCanvasGroup, true);
            SetCanvasState(_continueCanvasGroup, false);
            SetCanvasState(_mainMenuCanvasGroup, false);
        }

        private void SetCanvasState(CanvasGroup canvas, bool active)
        {
            canvas.alpha = active ? 1f : 0f;
            canvas.interactable = active;
            canvas.blocksRaycasts = active;
        }
    }
}
