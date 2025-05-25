using System;
using UnityEngine;

[Serializable]
public class ClearWeaponData
{
    public ClearWeaponData(WeaponData weaponData)
    {
        BaseDamage = weaponData.baseDamage;
        UpgradeLevel = weaponData.upgradeLevel;
        CurrentDamage = weaponData.currentDamage;
    }

    public int BaseDamage;
    public int UpgradeLevel;
    public int CurrentDamage;
}