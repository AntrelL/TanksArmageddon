using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] _weaponDataList = new WeaponData[5];

    private int[] _weaponCardCounts = new int[5];
    //[SerializeField] private int _playerBalance;
    //[SerializeField] private int _playerPoints;
    //[SerializeField] private int _playerHealth;

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
        UIController.PlayerPointsReceived += SetPlayerPoints;
    }

    private void OnDisable()
    {
        UIController.PlayerRewardReceived -= SetPlayerBalance;
        UIController.PlayerPointsReceived -= SetPlayerPoints;
    }

    public void IncreasePlayerHealth()
    {
        YG2.saves.playerHealth += 10;
    }

    public int GetPlayerHealth()
    {
        return YG2.saves.playerHealth;
    }

    private void SetPlayerPoints(int value)
    {
        YG2.saves.playerPoints += value;
        Debug.Log($"Игроку было добавлено {value} поинтов. Текущий рейтинг: {YG2.saves.playerPoints}");
    }

    public int GetPlayerPoints()
    {
        return YG2.saves.playerPoints;
    }

    public int GetPlayerMaxHealth()
    {
        return YG2.saves.playerHealth;
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
        return YG2.saves.playerBalance;
    }

    public void SetPlayerBalance(int amount)
    {
        YG2.saves.playerBalance += amount;
        Debug.Log($"Игроку было добавлено {amount} денег.");
    }

    public bool TrySpendMoney(int amount)
    {
        if (YG2.saves.playerBalance >= amount)
        {
            YG2.saves.playerBalance -= amount;

            return true;
        }
        else
        {
            return false;
        }
    }
}
