using UnityEngine;

namespace Source.Scripts.Realese.Projectiles
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public new string Name { get; set; }

        [field: SerializeField] public Sprite Icon { get; set; }

        [field: SerializeField] public int BaseDamage { get; set; }

        [field: SerializeField] public int UpgradeLevel { get; set; }

        [field: SerializeField] public int CurrentDamage { get; set; }

        private void OnEnable()
        {
            CurrentDamage = BaseDamage;
        }
    }
}