using System.Collections;
using Source.Scripts.Release.Stuff;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Release.UI
{
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.05f;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private ShopSceneNavigationManager _shopSceneNavigationManager;
        [SerializeField] private HangarSceneNavigationManager _hangarSceneNavigationManager;

        private string _fullText;

        private void OnEnable()
        {
            if (_shopSceneNavigationManager != null && _hangarSceneNavigationManager != null)
            {
                _shopSceneNavigationManager.TextShowing += GetText;
                _hangarSceneNavigationManager.TextShowing += GetText;
            }
        }

        private void OnDisable()
        {
            if (_shopSceneNavigationManager != null && _hangarSceneNavigationManager != null)
            {
                _shopSceneNavigationManager.TextShowing -= GetText;
                _hangarSceneNavigationManager.TextShowing -= GetText;
            }
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
}