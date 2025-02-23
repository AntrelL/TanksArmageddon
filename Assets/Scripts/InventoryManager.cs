using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Button _toggleInventoryButton;
    [SerializeField] private List<WeaponSlot> _weaponSlots;
    [SerializeField] private List<WeaponData> _weaponsList;

    private bool isInventoryVisible = false;
    private WeaponSlot selectedSlot = null;

    public static event Action<int> UpdatePlayerDamage;

    private void Start()
    {
        //_toggleInventoryButton.onClick.AddListener(ToggleInventory);
        UpdateInventoryUI();
        SelectWeapon(_weaponSlots[0]);

        foreach (var slot in _weaponSlots)
        {
            var button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => SelectWeapon(slot));
        }
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        _inventoryPanel.SetActive(isInventoryVisible);
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            WeaponData weapon = _weaponsList[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }
    }

    public void SelectWeapon(WeaponSlot slot)
    {
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
        }

        selectedSlot = slot;

        int currentDamage = int.Parse(slot.currentDamage.text);

        UpdatePlayerDamage(currentDamage);
        selectedSlot.Select();
    }
}
