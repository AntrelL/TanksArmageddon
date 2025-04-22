using System.Collections.Generic;
using UnityEngine;

namespace TanksArmageddon
{
    public class ScreenSwitch : Script
    {
        [SerializeField] private Screen _switchPanel;
        [SerializeField] private List<ScreenSwitchPosition> _positions;
        [SerializeField] private int _homeScreenPositionIndex;

        private ScreenSwitchPosition _currentPosition = null;

        public void Initialize()
        {
            _switchPanel.Initialize();
            _positions.ForEach(button => button.Initialize());

            OnInitialized();
        }

        public void Open()
        {
            _switchPanel.Open();
            SelectPosition(_positions[_homeScreenPositionIndex]);
        }

        public void Close()
        {
            _switchPanel.Close();
            _currentPosition.Deselect();
        }

        public void SelectPosition(ScreenSwitchPosition position)
        {
            _currentPosition?.Deselect();
            position.Select();
            _currentPosition = position;
        }
    }
}
