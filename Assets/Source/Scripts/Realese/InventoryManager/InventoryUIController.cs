using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Source.Scripts.Release.InventoryManager
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private List<WeaponSlot> _weaponSlots;

        public void UpdateUI()
        {
            for (int i = 0; i < _weaponSlots.Count; i++)
            {
                var weapon = YG2.saves.ClearWeaponsData[i];
                _weaponSlots[i].SetWeaponData(weapon);
            }
        }

        public void HideAllExceptFirst()
        {
            for (int i = 1; i < _weaponSlots.Count; i++)
                _weaponSlots[i].gameObject.SetActive(false);
        }

        public WeaponSlot GetSlot(int index) => _weaponSlots[index];
    
        public List<WeaponSlot> Slots => _weaponSlots;
    }
}