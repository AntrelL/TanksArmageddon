namespace IJunior.UI
{
    public abstract class BasicDigitalText : BasicText, ILinkableText<float>
    {
        public float Value { get; protected set; }

        public virtual void SetValue(float value)
        {
            Value = value;
            SetText(value.ToString());
        }
    }
}