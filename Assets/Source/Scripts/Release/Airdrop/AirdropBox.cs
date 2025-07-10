using System;
using Source.Scripts.Release.HitProcessing;
using Source.Scripts.Release.Player;
using UnityEngine;

namespace Source.Scripts.Release.Airdrop
{
    public class AirdropBox : MonoBehaviour
    {
        public event Action<int> PlayerPickedUpAirdrop;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IImpactTarget impactTarget) == false)
                return;
            
            if (impactTarget is PlayerRoot)
                PlayerPickedUpAirdrop?.Invoke(GenerateRandomWeaponIndex());

            Destroy(gameObject);
        }

        private int GenerateRandomWeaponIndex()
        {
            int randomIndex = UnityEngine.Random.Range(1, 5);

            return randomIndex;
        }
    }
}