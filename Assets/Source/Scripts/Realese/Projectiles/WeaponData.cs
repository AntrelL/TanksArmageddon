using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string name;
    public Sprite icon;
    public int baseDamage;
    public int upgradeLevel = 0;
    public int currentDamage;

    private void OnEnable()
    {
        currentDamage = baseDamage;
    }
}
