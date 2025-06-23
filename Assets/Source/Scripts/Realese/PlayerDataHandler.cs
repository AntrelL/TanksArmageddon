using UnityEngine;
using YG;

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler Instance { get; private set; }

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
        YG2.saves.PlayerHealth += 10;
    }

    public int GetPlayerHealth()
    {
        return YG2.saves.PlayerHealth;
    }

    private void SetPlayerPoints(int value)
    {
        YG2.saves.PlayerPoints += value;
    }

    public int GetPlayerPoints()
    {
        return YG2.saves.PlayerPoints;
    }

    public int GetPlayerMaxHealth()
    {
        return YG2.saves.PlayerHealth;
    }

    public int GetPlayerBalance()
    {
        return YG2.saves.PlayerBalance;
    }

    public void SetPlayerBalance(int amount)
    {
        YG2.saves.PlayerBalance += amount;
        YG2.SaveProgress();
    }

    public bool TrySpendMoney(int amount)
    {
        if (YG2.saves.PlayerBalance >= amount)
        {
            YG2.saves.PlayerBalance -= amount;

            return true;
        }
        else
        {
            return false;
        }
    }
}