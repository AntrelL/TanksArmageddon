using IJunior.CompositeRoot;
using TMPro;
using UnityEngine;

namespace IJunior.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public abstract class BasicText : Script
    {
        [SerializeField] private bool _useDefaultTextAsPrefix = true;
        [SerializeField] private string _undefinedValueText;

        private string _prefixText;
        private TMP_Text _tmpText;

        protected string Text => _tmpText.text;

        public void SetUndefinedValue() => SetText(_undefinedValueText);

        protected void Initialize()
        {
            _tmpText = GetComponent<TMP_Text>();
            _prefixText = _useDefaultTextAsPrefix ? _tmpText.text : string.Empty;
        }

        protected void SetText(string value)
        {
            _tmpText.text = _prefixText + value;
        }
    }
}