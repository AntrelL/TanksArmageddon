using IJunior.CompositeRoot;
using UnityEngine;

namespace IJunior.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Screen : Script
    {
        [SerializeField] private bool _isActiveByDefault;

        private CanvasGroup _canvasGroup;

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetDefaultState()
        {
            if (_isActiveByDefault)
                Open();
            else
                Close();
        }

        public void SwitchTo(Screen targetScreen)
        {
            Close();
            targetScreen.Open();
        }

        public void Open() => SetState(true);

        public void Close() => SetState(false);

        private void SetState(bool value)
        {
            _canvasGroup.alpha = value ? 1 : 0;
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }
    }
}