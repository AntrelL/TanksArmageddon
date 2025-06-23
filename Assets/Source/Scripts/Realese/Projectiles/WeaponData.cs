using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public new string Name;
    public Sprite Icon;
    public int BaseDamage;
    public int UpgradeLevel = 0;
    public int CurrentDamage;

    private void OnEnable()
    {
        CurrentDamage = BaseDamage;
    }
}