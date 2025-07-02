using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] private InventoryUIController _ui;
    public event Action<int> OnWeaponSelected;

    private WeaponSlot _selectedSlot;
    private WeaponSlot _slotToClean;

    public void Initialize(Action onProjectileDestroyed)
    {
        foreach (var slot in _ui.Slots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() => Select(slot, onProjectileDestroyed));
        }
    }

    public void Select(WeaponSlot slot, Action onProjectileDestroyed)
    {
        _selectedSlot?.Deselect();
        _selectedSlot = slot;
        _selectedSlot.Select();

        int damage = int.Parse(slot.CurrentDamage.text);
        OnWeaponSelected?.Invoke(damage);

        if (slot.name != "Slot01")
        {
            _slotToClean = slot;
            DefaultProjectile.ProjectileDestroyed += onProjectileDestroyed;
        }
        else
        {
            _slotToClean = null;
        }
    }

    public WeaponSlot SlotToClean => _slotToClean;

    public void DeselectCurrent() => _selectedSlot?.Deselect();
    public void SelectFirst() => Select(_ui.GetSlot(0), null);
}