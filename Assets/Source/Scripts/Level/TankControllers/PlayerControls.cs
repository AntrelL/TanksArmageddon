using System;
using UnityEngine;

namespace TanksArmageddon
{
    [Serializable]
    public class PlayerControls
    {
        [field: SerializeField] public ButtonPressListener LeftMoveButton { get; private set; }

        [field: SerializeField] public ButtonPressListener RightMoveButton { get; private set; }
    }
}
