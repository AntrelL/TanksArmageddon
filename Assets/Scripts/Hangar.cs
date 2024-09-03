using UnityEngine;

public class Hangar : MonoBehaviour
{
    private WeaponUpgradeSystem _upgradeSystem;

    private void Start()
    {
        // Получаем компонент WeaponUpgradeSystem
        _upgradeSystem = GetComponent<WeaponUpgradeSystem>();

        if (_upgradeSystem == null)
        {
            Debug.LogError("WeaponUpgradeSystem component is not attached to the Hangar object.");
        }
    }

    // Метод для выбора и улучшения оружия по индексу
    public void SelectAndUpgradeWeapon(int weaponIndex)
    {
        WeaponData weaponData = GameManager.Instance.GetWeaponData(weaponIndex);

        if (weaponData != null)
        {
            // Устанавливаем данные текущего оружия и количество карточек
            _upgradeSystem.SetWeaponData(weaponData);
            _upgradeSystem.SetCards(GameManager.Instance.GetCardCount(weaponIndex));

            // Пытаемся улучшить оружие
            _upgradeSystem.UpgradeWeapon();

            // Обновляем количество карточек в GameManager после улучшения
            int remainingCards = _upgradeSystem.GetCurrentCards();
            GameManager.Instance.SetCardCount(weaponIndex, remainingCards);

            Debug.Log($"Selected and upgraded weapon: {weaponData.weaponName} to level {weaponData.upgradeLevel} with damage: {weaponData.baseDamage}");
        }
        else
        {
            Debug.LogError("Weapon data not found for the given weapon index.");
        }
    }
}
