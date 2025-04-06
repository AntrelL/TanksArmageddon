using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private AdService _adService;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private List<WeaponSlot> _weaponSlots;
    [SerializeField] private List<WeaponData> _weaponsList;

    private Dictionary<int, int> _weaponAmmoCount = new Dictionary<int, int>();

    private WeaponSlot _selectedSlot = null;
    private WeaponSlot _weaponSlotToClean;

    public static event Action<int> UpdatePlayerDamage;

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

        _weaponSlots[index].UpdateAmmoCount(_weaponAmmoCount[index]);
    }

    private void UpdateInventoryValues()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            WeaponData weapon = _weaponsList[i];
            _weaponSlots[i].SetWeaponData(weapon);
        }
    }

    private int GenerateRandomIndex()
    {
        int randomIndex = UnityEngine.Random.Range(1, 5);

        return randomIndex;
    }

    public void AdButtonPressed()
    {
        string hyiPoimi = "hyi";
#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.RewardedAdvShow(hyiPoimi, () =>
        {
            SetNewWeapon(GenerateRandomIndex());
        });
#endif
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
            int index = _weaponSlots.IndexOf(_weaponSlotToClean);

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
