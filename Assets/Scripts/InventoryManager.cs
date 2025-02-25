using DG.Tweening;
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
    private WeaponSlot _selectedSlot = null;
    private WeaponSlot _weaponSlotToClean;

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

    private void OnEnable()
    {
        AirdropBox.PlayerPickedUpAirdrop += SetAirDropPickedUpWeapon;
    }

    private void OnDisable()
    {
        AirdropBox.PlayerPickedUpAirdrop -= SetAirDropPickedUpWeapon;
    }

    private void SetAirDropPickedUpWeapon(int index)
    {
        UpdateInventoryValues();

        if (index == 1)
        {
            _weaponSlots[1].gameObject.SetActive(true);

        }

        if (index == 2)
        {
            _weaponSlots[2].gameObject.SetActive(true);
        }

        if (index == 3)
        {
            _weaponSlots[3].gameObject.SetActive(true);
        }

        if (index == 4)
        {
            _weaponSlots[4].gameObject.SetActive(true);
        }
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        _inventoryPanel.SetActive(isInventoryVisible);
    }

    private void UpdateInventoryValues()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            WeaponData weapon = _weaponsList[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            WeaponData weapon = _weaponsList[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }

        for (int i = 1; i < _weaponSlots.Count; i++)
        {
            _weaponSlots[i].gameObject.SetActive(false);
        }
    }

    public void SelectWeapon(WeaponSlot slot)
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.Deselect();
        }

        _selectedSlot = slot;

        int currentDamage = int.Parse(slot.currentDamage.text);

        UpdatePlayerDamage(currentDamage);
        _selectedSlot.Select();
        Debug.Log("selected slot name: " + slot.name);

        if (slot.name != "Slot01")
        {
            _weaponSlotToClean = slot;
            DefaultProjectile.ProjectileDestroyed += SetSelectedSlotInvisible;
        }
    }

    private void SetSelectedSlotInvisible()
    {
        Debug.Log("Hide slot with name:  " + _weaponSlotToClean.name);
        _weaponSlotToClean.gameObject.SetActive(false);
        SelectWeapon(_weaponSlots[0]);
        DefaultProjectile.ProjectileDestroyed -= SetSelectedSlotInvisible;
    }
}
