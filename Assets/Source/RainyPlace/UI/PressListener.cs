using RainyPlace.DI;
using UnityEngine.EventSystems;

namespace RainyPlace.UI
{
    public class PressListener : MonoScript, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
    }
}
