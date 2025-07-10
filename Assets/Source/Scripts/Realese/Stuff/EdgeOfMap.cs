using System;
using Source.Scripts.Release.Player;
using UnityEngine;

namespace Source.Scripts.Release.Stuff
{
    public class EdgeOfMap : MonoBehaviour
    {
        private int _damage = 5000;

        public event Action<int> CollisionWithPlayer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerRoot player))
            {
                CollisionWithPlayer?.Invoke(_damage);
            }
        }
    }
}
