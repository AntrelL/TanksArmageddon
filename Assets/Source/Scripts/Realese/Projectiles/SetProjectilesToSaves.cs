using System;
using UnityEngine;
using YG;

namespace Source.Scripts.Realese.Projectiles
{
    public class SetProjectilesToSaves : MonoBehaviour
    {
        [SerializeField] private WeaponData[] _weapons;

        private void Awake()
        {
            if (YG2.saves.ClearWeaponsData == null)
            {
                YG2.saves.ClearWeaponsData = Array.ConvertAll(_weapons, weapon => new ClearWeaponData(weapon));
                YG2.SaveProgress();
                Debug.Log("Converted weapons and save progress!");
            }
        }
    }
}