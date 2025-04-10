

using UnityEngine;

public class ClearWeaponData
{
    public ClearWeaponData(WeaponData weaponData)
    {
        BaseDamage = weaponData.baseDamage;
        UpgradeLevel = weaponData.upgradeLevel;
        CurrentDamage = weaponData.currentDamage;
        Icon = weaponData.icon;
    }

    public int BaseDamage { get; set; }
    public int UpgradeLevel { get; set; }
    public int CurrentDamage { get; set; }
    public Sprite Icon { get; set; }
}