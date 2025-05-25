using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
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


    public int GetPlayerMaxHealth()
    {
        return YG2.saves.playerHealth;
    }


    public int GetPlayerBalance()
    {
        return YG2.saves.playerBalance;
    }

    public void SetPlayerBalance(int amount)
    {
        YG2.saves.playerBalance += amount;
        YG2.SaveProgress();
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
