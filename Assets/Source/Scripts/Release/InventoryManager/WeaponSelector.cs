using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Release.InventoryManager
{
    public class WeaponSelector : MonoBehaviour
    {
        [SerializeField] private InventoryUIController _ui;

        private WeaponSlot _selectedSlot;
        private WeaponSlot _slotToClean;

        public event Action<int> WeaponSelected;

        public WeaponSlot SlotToClean => _slotToClean;

        public void Initialize()
        {
            foreach (var slot in _ui.Slots)
            {
                slot.GetComponent<Button>().onClick.AddListener(() => Select(slot));
            }
        }

        public void Select(WeaponSlot slot)
        {
            DeselectCurrent();

            _selectedSlot = slot;
            _selectedSlot.Select();

            int damage = int.Parse(slot.CurrentDamage.text);
            WeaponSelected?.Invoke(damage);

            if (slot.name != "Slot01")
            {
                _slotToClean = slot;
            }
            else
            {
                _slotToClean = null;
            }
        }

        public void SelectFirst() =>
            Select(_ui.GetSlot(0));

        public void DeselectCurrent()
        {
            if (_selectedSlot != null)
                _selectedSlot.Deselect();
        }
    }
}