using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.SOLID
{
    public class PlayerControls
    {
        [field: SerializeField] public PressListener MoveLeftButton { get; private set; }

        [field: SerializeField] public PressListener MoveRightButton { get; private set; }

        [field: SerializeField] public Button ShootButton { get; private set; }
    }
}
