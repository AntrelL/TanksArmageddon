using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Release.InventoryManager
{
    public class AmmoManager : MonoBehaviour
    {
        private readonly Dictionary<int, int> _ammo = new Dictionary<int, int>();

        public void AddAmmo(int index)
        {
            if (_ammo.ContainsKey(index))
                _ammo[index]++;
            else
                _ammo[index] = 1;
        }

        public bool UseAmmo(int index)
        {
            if (!_ammo.ContainsKey(index)) return false;

            if (_ammo[index] > 1)
            {
                _ammo[index]--;
                return true;
            }

            _ammo.Remove(index);

            return false;
        }

        public int GetAmmo(int index) => _ammo.TryGetValue(index, out int count) ? count : 0;
    }
}