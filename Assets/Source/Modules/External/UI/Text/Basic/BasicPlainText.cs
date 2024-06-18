namespace IJunior.UI
{
    public abstract class BasicPlainText : BasicText, ILinkableText<string>
    {
        public string Value => Text;

        public void SetValue(string value) => SetText(value);
    }
}