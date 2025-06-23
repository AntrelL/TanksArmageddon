using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float _delay = 0.05f;
    [SerializeField] private TMP_Text _text;
    private string _fullText;

    private void OnEnable()
    {
        ShopSceneNavigationManager.TextShowing += GetText;
        HangarSceneNavigationManager.TextShowing += GetText;
    }

    private void OnDisable()
    {
        ShopSceneNavigationManager.TextShowing -= GetText;
        HangarSceneNavigationManager.TextShowing -= GetText;
    }

    public void GetText(TMP_Text tmpText)
    {
        _fullText = tmpText.text;
        tmpText.text = string.Empty;
        StartCoroutine(ShowText(tmpText));
    }

    private void GetText()
    {
        _fullText = _text.text;
        _text.text = string.Empty;
        StartCoroutine(ShowText(_text));
    }

    private IEnumerator ShowText(TMP_Text tmpText)
    {
        foreach (char letter in _fullText)
        {
            tmpText.text += letter;

            yield return new WaitForSeconds(_delay);
        }
    }
}