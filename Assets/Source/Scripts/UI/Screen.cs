using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Screen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    public void Construct()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open() => SetCanvasGroupActive(true);

    public void Close() => SetCanvasGroupActive(false);

    public void SwitchTo(Screen screen)
    {
        Close();
        screen.Open();
    }

    private void SetCanvasGroupActive(bool isActivate)
    {
        _canvasGroup.alpha = isActivate ? 1 : 0;
        _canvasGroup.interactable = isActivate;
        _canvasGroup.blocksRaycasts = isActivate;
    }
}
