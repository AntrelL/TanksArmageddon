using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int baseDamage;  // Текущий урон оружия
    public int upgradeLevel = 0;

    [HideInInspector] public int initialDamage; // Новое поле для хранения начального урона

    private void OnEnable()
    {
        // Сохраняем начальный урон при создании экземпляра
        initialDamage = baseDamage;
    }
}
