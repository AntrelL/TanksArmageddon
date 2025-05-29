using System;

namespace RainyPlace.Core
{
    public interface IProtectedHealth
    {
        IProtectedEvent<Action> Died { get; }

        IProtectedEvent<Action<int>> Changed { get; }

        IProtectedEvent<Action<IReadOnlyScale<int>>> ScaleChanged { get; }

        void Heal(int value);

        void TakeDamage(int value);
    }
}
