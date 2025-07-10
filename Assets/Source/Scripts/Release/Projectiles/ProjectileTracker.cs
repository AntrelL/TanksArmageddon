using System;
using UnityEngine;

namespace Source.Scripts.Release.Projectiles
{
    public class ProjectileTracker : MonoBehaviour
    {
        public static ProjectileTracker Instance { get; private set; }

        public Transform CurrentProjectile { get; private set; }

        public event Action ProjectileDestroyed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterProjectile(Transform projectile)
        {
            CurrentProjectile = projectile;
        }

        public void ClearProjectile()
        {
            CurrentProjectile = null;
            ProjectileDestroyed?.Invoke();
        }
    }
}