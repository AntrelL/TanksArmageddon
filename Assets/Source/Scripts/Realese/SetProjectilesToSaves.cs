using System;
using UnityEngine;
using YG;

public class SetProjectilesToSaves : MonoBehaviour
{
    [SerializeField] private WeaponData[] _weapons;

    private void Start()
    {
        if (YG2.saves.ClearWeaponsData == null)
        {
            YG2.saves.ClearWeaponsData = Array.ConvertAll(_weapons, weapon => new ClearWeaponData(weapon));
            YG2.SaveProgress();
        }
    }
}