using System;

[Serializable]
public class ClearWeaponData
{
    public ClearWeaponData(WeaponData weaponData)
    {
        BaseDamage = weaponData.BaseDamage;
        UpgradeLevel = weaponData.UpgradeLevel;
        CurrentDamage = weaponData.CurrentDamage;
    }

    public int BaseDamage;
    public int UpgradeLevel;
    public int CurrentDamage;
}