using UnityEngine.EventSystems;
using TanksArmageddon.CompositeRoot;

namespace TanksArmageddon.UI
{
    public class ButtonPressListener : MonoScript, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; } = false;

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
    }
}
