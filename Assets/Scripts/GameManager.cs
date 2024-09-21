using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int[] _weaponCardCounts = new int[5]; // Количество карточек для каждого оружия
    [SerializeField] private WeaponData[] _weaponDataList = new WeaponData[5]; // Данные для каждого оружия
    private int _playerBalance = 100000; // Изначально у игрока 1000 монет (можно изменить)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Получение количества карточек для определенного оружия
    public int GetCardCount(int weaponIndex)
    {
        return _weaponCardCounts[weaponIndex];
    }

    // Установка количества карточек для определенного оружия
    public void SetCardCount(int weaponIndex, int cardCount)
    {
        _weaponCardCounts[weaponIndex] = cardCount;
    }

    // Получение данных оружия по индексу
    public WeaponData GetWeaponData(int weaponIndex)
    {
        return _weaponDataList[weaponIndex];
    }

    // Добавление данных оружия в список
    public void SetWeaponData(int weaponIndex, WeaponData data)
    {
        _weaponDataList[weaponIndex] = data;
    }

    // Получение баланса игрока
    public int GetPlayerBalance()
    {
        return _playerBalance;
    }

    // Установка баланса игрока
    public void SetPlayerBalance(int newBalance)
    {
        _playerBalance = newBalance;
    }

    // Метод для уменьшения баланса (используется при покупке)
    public bool TrySpendMoney(int amount)
    {
        if (_playerBalance >= amount)
        {
            _playerBalance -= amount;
            return true; // Покупка успешна
        }
        else
        {
            return false; // Недостаточно средств
        }
    }
}
