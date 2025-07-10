using System;
using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.Airdrop
{
    public class AirdropBox : MonoBehaviour
    {
        public event Action<int> PlayerPickedUpAirdrop;

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

        private int GenerateRandomWeaponIndex()
        {
            int randomIndex = UnityEngine.Random.Range(1, 5);

            return randomIndex;
        }
    }
}