using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.SOLID
{
    public class PressListener : IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
    }
}
