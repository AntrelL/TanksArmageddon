using System;

namespace TanksArmageddon
{
    public interface IReadOnlyScale
    {
        public event Action<float> ValueChanged;

        public float MinValue { get; }

        public float MaxValue { get; }

        public float Value { get; }

        public bool IsFull { get; }

        public bool IsEmpty { get; }
    }
}
