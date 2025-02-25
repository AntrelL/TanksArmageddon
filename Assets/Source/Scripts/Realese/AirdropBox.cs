using System;
using TanksArmageddon;
using UnityEngine;

public class AirdropBox : MonoBehaviour
{
    public event Action<GameObject> OnAirDropCollected;
    public static event Action<int> PlayerPickedUpAirdrop;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Land land))
        {
            Debug.Log("Касаемся только земли!");
            return;
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            OnAirDropCollected?.Invoke(collision.gameObject);
            PlayerPickedUpAirdrop?.Invoke(GenerateRandomWeaponIndex());
            Debug.Log("Если соприкоснулись с игроком");
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            OnAirDropCollected?.Invoke(collision.gameObject);
            Destroy(gameObject);
            Debug.Log("Если соприкоснулись с врагом");
            return;
        }

        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            Destroy(gameObject);
        }
    }

    private int GenerateRandomWeaponIndex()
    {
        int randomIndex = UnityEngine.Random.Range(1, 5);

        return randomIndex;
    }
}
