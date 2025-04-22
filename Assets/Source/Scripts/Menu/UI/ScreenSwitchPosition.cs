using UnityEngine;
using UnityEngine.UI;

namespace TanksArmageddon
{
    public class ScreenSwitchPosition : Script
    {
        [SerializeField] private Button _button;

        [field: SerializeField] public Screen LinkedScreen { get; private set; }

        public void Initialize()
        {
            if (LinkedScreen.IsInitialized == false)
                LinkedScreen.Initialize();

            OnInitialized();
        }

        public void Select()
        {
            LinkedScreen.Open();
            _button.interactable = false;
        }

        public void Deselect()
        {
            LinkedScreen.Close();
            _button.interactable = true;
        }
    }
}
