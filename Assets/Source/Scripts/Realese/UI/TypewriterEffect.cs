using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float _delay = 0.05f;
    private string _fullText;

    public void GetText(TMP_Text TMPtext)
    {
        _fullText = TMPtext.text;
        TMPtext.text = "";
        StartCoroutine(ShowText(TMPtext));
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
