using IJunior.CompositeRoot;

namespace IJunior.UI
{
    public class TextLinker<T> : IActivatable
    {
        private ILinkableText<T> _linkableText;
        private ITextLinkerSource<T> _source;

        public void Initialize(
            ILinkableText<T> linkableText,
            ITextLinkerSource<T> source)
        {
            _linkableText = linkableText;
            _source = source;

            OnValueChanged(_source.Value);
        }

        public void OnActivate()
        {
            _source.ValueChanged += OnValueChanged;
        }

        public void OnDeactivate()
        {
            _source.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(T value) => _linkableText.SetValue(value);
    }
}