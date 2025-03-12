using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] _weaponDataList = new WeaponData[5];

    private int[] _weaponCardCounts = new int[5];
    [SerializeField] private int _playerBalance;

    public static GameManager Instance { get; private set; }

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

    private void OnEnable()
    {
        UIController.PlayerRewardReceived += SetPlayerBalance;
    }

    private void OnDisable()
    {
        UIController.PlayerRewardReceived -= SetPlayerBalance;
    }

    public int GetCardCount(int weaponIndex)
    {
        return _weaponCardCounts[weaponIndex];
    }

    public void SetCardCount(int weaponIndex, int cardCount)
    {
        _weaponCardCounts[weaponIndex] = cardCount;
    }

    public WeaponData GetWeaponData(int weaponIndex)
    {
        return _weaponDataList[weaponIndex];
    }

    public void SetWeaponData(int weaponIndex, WeaponData data)
    {
        _weaponDataList[weaponIndex] = data;
    }

    public int GetPlayerBalance()
    {
        return _playerBalance;
    }

    public void SetPlayerBalance(int amount)
    {
        _playerBalance += amount;
        Debug.Log($"Игроку было добавлено {amount} денег.");
    }

    public bool TrySpendMoney(int amount)
    {
        if (_playerBalance >= amount)
        {
            _playerBalance -= amount;

            return true;
        }
        else
        {
            return false;
        }
    }
}
