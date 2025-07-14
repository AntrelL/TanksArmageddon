using System;
using System.Collections.Generic;
using Source.Scripts.Release.Airdrop;
using Source.Scripts.Release.Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Release.InventoryManager
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryUIController _ui;
        [SerializeField] private WeaponSelector _selector;
        [SerializeField] private AmmoManager _ammo;
        [SerializeField] private AirdropSpawner _airdropSpawner;
        [SerializeField] private List<WeaponData> _weaponDataList;
        [SerializeField] private ProjectileTracker _projectileTracker;

        public event Action<int> UpdatePlayerDamage;

        private void Awake()
        {
            _projectileTracker.ProjectileDestroyed += OnProjectileDestroyed;
        }

        private void Start()
        {
            _ui.UpdateUI();
            _ui.HideAllExceptFirst();

            _selector.WeaponSelected += OnWeaponSelected;
            _selector.Initialize();
            _selector.SelectFirst();
        }

        private void OnEnable()
        {
            _airdropSpawner.Spawned += OnAirdropSpawned;
        }

        private void OnDisable()
        {
            _airdropSpawner.Spawned -= OnAirdropSpawned;
        }

        private void OnDestroy()
        {
            if (_projectileTracker != null)
            {
                _projectileTracker.ProjectileDestroyed -= OnProjectileDestroyed;
            }

            _selector.WeaponSelected -= OnWeaponSelected;
        }

        public void AdButtonPressed()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.RewardedAdvShow("ad", () =>
        {
            SetNewWeapon(GenerateRandomIndex());
        });
#endif
        }

        private void SetNewWeapon(int index)
        {
            _ammo.AddAmmo(index);

            var slot = _ui.GetSlot(index);
            slot.gameObject.SetActive(true);

            var weaponData = _weaponDataList[index];
            slot.Icon.sprite = weaponData.Icon;

            var weapon = new ClearWeaponData(weaponData);
            slot.SetWeaponData(weapon);

            slot.UpdateAmmoCount(_ammo.GetAmmo(index));

            _selector.DeselectCurrent();
            _selector.Select(slot);
        }

        private void OnProjectileDestroyed()
        {
            var slotToClean = _selector.SlotToClean;

            if (slotToClean == null) return;

            int index = _ui.Slots.IndexOf(slotToClean);

            if (_ammo.UseAmmo(index))
            {
                slotToClean.UpdateAmmoCount(_ammo.GetAmmo(index));
                _selector.SelectFirst();
            }
            else
            {
                slotToClean.gameObject.SetActive(false);
                _selector.SelectFirst();
            }
        }

        private int GenerateRandomIndex() =>
            Random.Range(1, 5);

        private void OnWeaponSelected(int damage) =>
            UpdatePlayerDamage?.Invoke(damage);

        private void OnAirdropSpawned(AirdropBox airdrop)
        {
            airdrop.PlayerPickedUpAirdrop += OnPlayerPickedUpAirdrop;

            void OnPlayerPickedUpAirdrop(int weaponIndex)
            {
                SetNewWeapon(weaponIndex);
                airdrop.PlayerPickedUpAirdrop -= OnPlayerPickedUpAirdrop;
            }
        }
    }
}
