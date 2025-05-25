using System;
using TanksArmageddon;
using UnityEngine;

public class AirdropBox : MonoBehaviour
{
    public event Action<GameObject> OnAirDropCollected;
    public static event Action<int> PlayerPickedUpAirdrop;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            OnAirDropCollected?.Invoke(collision.gameObject);
            PlayerPickedUpAirdrop?.Invoke(GenerateRandomWeaponIndex());

            Destroy(gameObject);

            return;
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            OnAirDropCollected?.Invoke(collision.gameObject);
            Destroy(gameObject);

            return;
        }

        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Destroy(gameObject);

            return;
        }
    }

    private int GenerateRandomWeaponIndex()
    {
        int randomIndex = UnityEngine.Random.Range(1, 5);

        return randomIndex;
    }
}
