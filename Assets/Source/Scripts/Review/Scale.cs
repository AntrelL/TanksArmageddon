using System;

public abstract class Scale<T> : IReadOnlyScale<T>
{
    protected readonly Contract _valueInRangeContract = new("The value is out of range, ");

    private T _value;
    private T _min;
    private T _max;

    private bool _autoRangeLimitation;
    private Realese.Event<T> _changed;

    public Scale(T value, T min, T max, bool autoRangeLimitation = false)
    {
        _autoRangeLimitation = autoRangeLimitation;

        SetValues(value, min, max);
        _changed = new();
    }

    public Scale(IReadOnlyScale<T> sample, bool autoRangeLimitation = false) :
        this(sample.Value, sample.Min, sample.Max, autoRangeLimitation)
    { }

    public T Value
    {
        get => _value;
        set => SetValues(value, _min, _max);
    }

    public T Min
    {
        get => _min;
        set => SetValues(_value, value, _max);
    }

    public T Max
    {
        get => _max;
        set => SetValues(_value, _min, value);
    }

    public IProtectedEvent<Action<T>> Changed => _changed;

    public override string ToString() => ToString(Value, Min, Max);

    protected abstract bool IsInRange(T value, T min, T max);

    protected abstract T Clamp(T value, T min, T max);

    private void SetValues(T value, T min, T max)
    {
        bool isInRange = IsInRange(value, min, max);
        string postfix = ToString(value, min, max);

        if (_autoRangeLimitation && isInRange == false)
        {
            value = Clamp(value, min, max);
            isInRange = true;
        }

        if (_valueInRangeContract.CheckViolation(isInRange == false, postfix: postfix))
            return;

        _value = value;
        _min = min;
        _max = max;
    }

    private string ToString(T value, T min, T max)
    {
        return $"Scale<{typeof(T).Name}> Value: {value}, Range: [{min};{max}]";
    }
}
