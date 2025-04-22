using UnityEngine;

namespace TanksArmageddon
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Screen : Script
    {
        [SerializeField] private bool _activateByDefault;

        private CanvasGroup _canvasGroup;

        public bool Activated { get; private set; }

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetStates(_activateByDefault);

            OnInitialized();
        }

        public void Open() => SetStates(true);

        public void Close() => SetStates(false);

        public void SwitchTo(Screen screen)
        {
            Close();
            screen.Open();
        }

        public void SetStates(bool activate)
        {
            _canvasGroup.blocksRaycasts = activate;
            _canvasGroup.interactable = activate;
            _canvasGroup.alpha = activate ? 1 : 0;

            Activated = activate;
        }
    }
}
