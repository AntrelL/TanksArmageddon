using UnityEngine;
using UnityEngine.EventSystems;

namespace TanksArmageddon
{
    // TODO: Move such scripts to separate modules
    public class PressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
    }
}
