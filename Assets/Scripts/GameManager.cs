using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int[] _weaponCardCounts = new int[5]; // ���������� �������� ��� ������� ������
    [SerializeField] private WeaponData[] _weaponDataList = new WeaponData[5]; // ������ ��� ������� ������
    private int _playerBalance = 100000; // ���������� � ������ 1000 ����� (����� ��������)

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

    // ��������� ���������� �������� ��� ������������� ������
    public int GetCardCount(int weaponIndex)
    {
        return _weaponCardCounts[weaponIndex];
    }

    // ��������� ���������� �������� ��� ������������� ������
    public void SetCardCount(int weaponIndex, int cardCount)
    {
        _weaponCardCounts[weaponIndex] = cardCount;
    }

    // ��������� ������ ������ �� �������
    public WeaponData GetWeaponData(int weaponIndex)
    {
        return _weaponDataList[weaponIndex];
    }

    // ���������� ������ ������ � ������
    public void SetWeaponData(int weaponIndex, WeaponData data)
    {
        _weaponDataList[weaponIndex] = data;
    }

    // ��������� ������� ������
    public int GetPlayerBalance()
    {
        return _playerBalance;
    }

    // ��������� ������� ������
    public void SetPlayerBalance(int newBalance)
    {
        _playerBalance = newBalance;
    }

    // ����� ��� ���������� ������� (������������ ��� �������)
    public bool TrySpendMoney(int amount)
    {
        if (_playerBalance >= amount)
        {
            _playerBalance -= amount;
            return true; // ������� �������
        }
        else
        {
            return false; // ������������ �������
        }
    }
}