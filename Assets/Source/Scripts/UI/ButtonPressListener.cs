using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed { get; private set; } = false;

    public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

    public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
}
