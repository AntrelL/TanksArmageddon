using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<WeaponSlot> _weaponSlots;
    [SerializeField] private List<WeaponData> _weaponsList;

    private WeaponSlot _selectedSlot;

    private readonly Dictionary<int, int> _weaponAmmoCount = new Dictionary<int, int>();
    private WeaponSlot _weaponSlotToClean;

    private void Start()
    {
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
        AirdropBox.PlayerPickedUpAirdrop += SetNewWeapon;
    }

    private void OnDisable()
    {
        AirdropBox.PlayerPickedUpAirdrop -= SetNewWeapon;
    }

    public static event Action<int> UpdatePlayerDamage;

    private void SetNewWeapon(int index)
    {
        UpdateInventoryValues();

        if (_weaponAmmoCount.ContainsKey(index))
        {
            _weaponAmmoCount[index]++;
        }
        else
        {
            _weaponAmmoCount[index] = 1;
            _weaponSlots[index].gameObject.SetActive(true);
        }

        _weaponSlots[index].GetComponent<Image>().sprite = _weaponsList[index].icon;
        _weaponSlots[index].UpdateAmmoCount(_weaponAmmoCount[index]);
    }

    private void UpdateInventoryValues()
    {
        for (var i = 0; i < _weaponSlots.Count; i++)
        {
            var weapon = YG2.saves.clearWeaponsData[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }
    }

    private int GenerateRandomIndex()
    {
        var randomIndex = Random.Range(1, 5);

        return randomIndex;
    }

    public void AdButtonPressed()
    {
        var ad = "ad";
#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.RewardedAdvShow(ad, () =>
        {
            SetNewWeapon(GenerateRandomIndex());
        });
#endif
    }

    public void UpdateInventoryUI()
    {
        for (var i = 0; i < _weaponSlots.Count; i++)
        {
            var weapon = YG2.saves.clearWeaponsData[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }

        for (var i = 1; i < _weaponSlots.Count; i++) _weaponSlots[i].gameObject.SetActive(false);
    }

    public void SelectWeapon(WeaponSlot slot)
    {
        if (_selectedSlot != null) _selectedSlot.Deselect();

        _selectedSlot = slot;

        var currentDamage = int.Parse(slot.currentDamage.text);
        UpdatePlayerDamage(currentDamage);
        _selectedSlot.Select();

        if (slot.name == "Slot01")
        {
            _weaponSlotToClean = null;
        }
        else
        {
            _weaponSlotToClean = slot;
            DefaultProjectile.ProjectileDestroyed += SetSelectedSlotInvisible;
        }
    }

    private void SetSelectedSlotInvisible()
    {
        if (_weaponSlotToClean != null)
        {
            var index = _weaponSlots.IndexOf(_weaponSlotToClean);

            if (_weaponAmmoCount.ContainsKey(index) && _weaponAmmoCount[index] > 1)
            {
                _weaponAmmoCount[index]--;
                _weaponSlots[index].UpdateAmmoCount(_weaponAmmoCount[index]);
                SelectWeapon(_weaponSlots[0]);
            }
            else
            {
                _weaponSlots[index].gameObject.SetActive(false);
                _weaponAmmoCount.Remove(index);
                SelectWeapon(_weaponSlots[0]);
            }
        }

        DefaultProjectile.ProjectileDestroyed -= SetSelectedSlotInvisible;
    }
}