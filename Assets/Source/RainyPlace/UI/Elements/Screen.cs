using RainyPlace.DI;
using UnityEngine;

namespace RainyPlace.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Screen : MonoScript
    {
        [SerializeField] private bool _enabledByDefault;

        private CanvasGroup _canvasGroup;

        public void Construct()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetValues(_enabledByDefault);

            OnConstructed();
        }

        public void Show() => SetValues(true);

        public void Hide() => SetValues(false);

        public void SwitchTo(Screen targetScreen)
        {
            Hide();
            targetScreen.Show();
        }

        public void SetValues(bool state)
        {
            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = state;
            _canvasGroup.alpha = state ? 1 : 0;
        }
    }
}
