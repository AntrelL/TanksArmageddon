using System;
using Source.Scripts.Release.HitProcessing;
using Source.Scripts.Release.Player;
using UnityEngine;

namespace Source.Scripts.Release.Stuff
{
    public class EdgeOfMap : MonoBehaviour, IImpactTarget
    {
        private const int Damage = 5000;

        public event Action<int> CollisionWithPlayer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerRoot player))
                CollisionWithPlayer?.Invoke(Damage);
        }
    }
}
