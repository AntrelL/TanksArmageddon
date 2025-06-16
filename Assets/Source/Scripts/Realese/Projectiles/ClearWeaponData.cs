using System;

[Serializable]
public class ClearWeaponData
{
    public int BaseDamage;
    public int UpgradeLevel;
    public int CurrentDamage;

    public ClearWeaponData(WeaponData weaponData)
    {
        BaseDamage = weaponData.baseDamage;
        UpgradeLevel = weaponData.upgradeLevel;
        CurrentDamage = weaponData.currentDamage;
    }
}