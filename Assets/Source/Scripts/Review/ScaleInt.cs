using System;

public class ScaleInt : Scale<int>
{
    public ScaleInt(int value, int min, int max, bool autoRangeLimitation = false) :
        base(value, min, max, autoRangeLimitation)
    { }

    public ScaleInt(IReadOnlyScale<int> sample, bool autoRangeLimitation = false) :
        base(sample, autoRangeLimitation)
    { }

    protected override bool IsInRange(int value, int min, int max) => value.IsInRange(min, max);

    protected override int Clamp(int value, int min, int max) => Math.Clamp(value, min, max);
}
