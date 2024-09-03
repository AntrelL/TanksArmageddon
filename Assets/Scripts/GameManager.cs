using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private WeaponData[] _weapons;  // —писок всех видов оружи€
    private int[] _cardCounts;  //  оличество карточек дл€ каждого вида оружи€

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _cardCounts = new int[_weapons.Length];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ћетод дл€ получени€ данных оружи€ по индексу
    public WeaponData GetWeaponData(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weapons.Length)
        {
            return _weapons[weaponIndex];
        }
        return null;
    }

    // ћетод дл€ добавлени€ карточек к выбранному оружию
    public void AddCards(int weaponIndex, int count)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCounts.Length)
        {
            _cardCounts[weaponIndex] += count;
            Debug.Log($"Added {count} cards to weapon index {weaponIndex}. Total: {_cardCounts[weaponIndex]} cards.");
        }
    }

    // ћетод дл€ установки количества карточек (например, после улучшени€)
    public void SetCardCount(int weaponIndex, int count)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCounts.Length)
        {
            _cardCounts[weaponIndex] = count;
            Debug.Log($"Set {count} cards for weapon index {weaponIndex}.");
        }
    }

    // ћетод дл€ получени€ количества карточек дл€ выбранного оружи€
    public int GetCardCount(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCounts.Length)
        {
            return _cardCounts[weaponIndex];
        }
        return 0;
    }
}
