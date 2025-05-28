using RainyPlace.DI;
using UnityEngine;
using UnityEngine.UI;

namespace RainyPlace.UI
{
    public class PlayerControls : MonoScript
    {
        [field: SerializeField] public PressListener MoveLeftButton { get; private set; }

        [field: SerializeField] public PressListener MoveRightButton { get; private set; }

        [field: SerializeField] public Button ShootButton { get; private set; }
    }
}
