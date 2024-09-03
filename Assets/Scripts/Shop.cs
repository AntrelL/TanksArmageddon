using UnityEngine;

public class Shop : MonoBehaviour
{
    // ћетод дл€ покупки карточек дл€ конкретного оружи€ по его индексу
    public void BuyCards(int weaponIndex)
    {
        if (GameManager.Instance != null)
        {
            // ѕровер€ем, существует ли WeaponData дл€ заданного индекса
            WeaponData weaponData = GameManager.Instance.GetWeaponData(weaponIndex);

            if (weaponData != null)
            {
                GameManager.Instance.AddCards(weaponIndex, 10);
                Debug.Log($"Bought {10} cards for weapon: {weaponData.weaponName}.");
            }
            else
            {
                Debug.LogError($"Weapon data not found for index: {weaponIndex}");
            }
        }
        else
        {
            Debug.LogError("GameManager instance is not available.");
        }
    }
}
