using System;

namespace Assets.Source.Scripts.SOLID
{
    public class Health
    {
        private int _amount;
        private int _min;
        private int _max;

        public Health(int amount, int min, int max)
        {
            _amount = amount;
            _min = min;
            _max = max;
        }

        public event Action Died;
        public event Action<int> Changed;

        public void Heal(int value)
        {
            if ((value < 0) || (_amount + value > _max))
                return;

            _amount += value;
            Changed?.Invoke(_amount);
        }

        public void TakeDamage(int value)
        {
            if (value < 0)
                return;

            if (_amount == _min)
            {
                Died?.Invoke();

                return;
            }

            if ((_amount == value) || (_amount - value <= 0))
            {
                Died?.Invoke();

                return;
            }

            _amount -= value;
            Changed?.Invoke(_amount);
        }
    }
}
