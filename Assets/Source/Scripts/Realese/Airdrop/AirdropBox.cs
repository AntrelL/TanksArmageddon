using System;
using TanksArmageddon;
using UnityEngine;

public class AirdropBox : MonoBehaviour
{
    public event Action<int> PlayerPickedUpAirdrop;
    public event Action PickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerRoot player))
        {
            PlayerPickedUpAirdrop?.Invoke(GenerateRandomWeaponIndex());
            Destroy(gameObject);

            return;
        }

        if (collision.gameObject.TryGetComponent(out EnemyFacade enemy))
        {
            Destroy(gameObject);

            return;
        }

        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Destroy(gameObject);

            return;
        }
    }

    public void OnDestroy()
    {
    }

    private int GenerateRandomWeaponIndex()
    {
        int randomIndex = UnityEngine.Random.Range(1, 5);

        return randomIndex;
    }
}