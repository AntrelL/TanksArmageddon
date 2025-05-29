using System;

namespace RainyPlace.Core
{
    public class Health : IProtectedHealth
    {
        private readonly Contract HealСontract = new("The healing value cannot be negative");
        private readonly Contract TakeDamageContract = new("The damage value cannot be negative");

        private ScaleInt _scale;
        private Event _died = new();
        private Event<IReadOnlyScale<int>> _scaleChanged = new();

        public Health(IReadOnlyScale<int> settings)
        {
            SetScaleSettins(settings);
        }

        public IProtectedEvent<Action> Died => _died;

        public IProtectedEvent<Action<int>> Changed => _scale.Changed;

        public IProtectedEvent<Action<IReadOnlyScale<int>>> ScaleChanged => _scaleChanged;

        public void SetScaleSettins(IReadOnlyScale<int> settings)
        {
            _scale = new(settings, true);
            _scaleChanged.Invoke(_scale);
        }

        public void Heal(int value)
        {
            if (HealСontract.CheckViolation(value < 0))
                return;

            _scale.Value += value;
        }

        public void TakeDamage(int value)
        {
            if (TakeDamageContract.CheckViolation(value < 0))
                return;

            if (_scale.Value == _scale.Min)
                return;

            _scale.Value -= value;

            if (_scale.Value == _scale.Min)
                _died.Invoke();
        }
    }
}
