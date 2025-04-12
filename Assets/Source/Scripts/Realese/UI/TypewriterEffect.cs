using System.Collections;
using System.Collections.Generic;
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

    public void GetText(TMP_Text TMPtext)
    {
        _fullText = TMPtext.text;
        TMPtext.text = "";
        StartCoroutine(ShowText(TMPtext));
    }

    private void GetText()
    {
        _fullText = _text.text;
        _text.text = "";
        StartCoroutine(ShowText(_text));
    }

    private IEnumerator ShowText(TMP_Text TMPtext)
    {
        foreach (char letter in _fullText)
        {
            TMPtext.text += letter;

            yield return new WaitForSeconds(_delay);
        }
    }
}
