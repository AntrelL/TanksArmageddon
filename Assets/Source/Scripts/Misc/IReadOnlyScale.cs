using System;

namespace TanksArmageddon
{
    public interface IReadOnlyScale
    {
        event Action<float> ValueChanged;

        float MinValue { get; }

        float MaxValue { get; }

        float Value { get; }

        bool IsFull { get; }

        bool IsEmpty { get; }
    }
}
